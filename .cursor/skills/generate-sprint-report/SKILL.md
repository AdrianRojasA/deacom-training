---
name: generate-sprint-report
description: Generates a bi-weekly sprint report Excel file from Jira data. Use when the user asks to generate a sprint report, create a sprint Excel, or says "sprint report". All supporting files (script, team config) live in the same folder as this skill.
---

# Generate Sprint Report

## Overview

Produces a colour-coded Excel sprint report for a given 2-week sprint period.
Colours:
- **Yellow** = Completed (status = Done, has resolution date)
- **Pink** = On Hold / Waiting (no resolution date, status = To Do)
- **Red (Orange)** = Carry Over (no resolution date, status = In Progress)

## Skill folder

All supporting files live alongside this skill at
**`.cursor/skills/generate-sprint-report/`**:

- `generate_sprint_report.py` — the Python script that produces the Excel file
- `sprint_team_config.md` — team roster, days off, holidays, Jira settings

Use the variable **`SKILL_DIR`** below to mean the absolute path to this folder
(resolve it at runtime, e.g. the directory containing this `SKILL.md` file).

## Configuration file

**`SKILL_DIR/sprint_team_config.md`** stores the team roster,
individual days off, team-wide holidays, and Jira connection settings.

Before fetching data, **always read this file first** to obtain:
- The list of team member emails (under `## Team Members`)
- Days off per person (under `## Days Off`)
- Team holidays (under `## Team Holidays`)
- Jira Cloud ID and Project key (under `## Jira Settings`)

If the user provides explicit email addresses in their message, use **only those
emails** instead of the full team list. Otherwise use every member from the
config.

## Inputs

| Input           | Required | Default / Source                                           |
| -----------------| ----------| ------------------------------------------------------------|
| `emails`        | no       | read from `sprint_team_config.md` `## Team Members`        |
| `sprint_end`    | no       | today                                                      |
| `sprint_start`  | no       | `sprint_end` minus 13 days (14-day window)                 |
| `jira_cloud_id` | no       | from `sprint_team_config.md` `## Jira Settings` → Cloud ID |
| `jira_project`  | no       | from `sprint_team_config.md` `## Jira Settings` → Project  |
| `inline_pto`    | no       | parsed from natural language in the user's message         |

If no dates are given, auto-calculate `sprint_start` and `sprint_end` to cover
the most recent completed 2-week period ending on the previous Saturday (or
today if today is Saturday).

### Parsing natural-language dates

The user may specify dates in plain English. Resolve them to `YYYY-MM-DD` using
the current date as context:

| User says | Interpret as |
|---|---|
| "for the 2nd to 13th of march" | `sprint_start = YYYY-03-02`, `sprint_end = YYYY-03-13` |
| "for only last week" | Previous Monday through Sunday |
| "for this week" | Current Monday through today |
| "from march 1 to march 14" | `sprint_start = YYYY-03-01`, `sprint_end = YYYY-03-14` |

When only a month and day range is given, assume the current year.

### Parsing inline PTO / days off

The user may include PTO information in the same message. When detected:

1. Match the person's name against the `## Team Members` table in config.
2. Expand the date range into individual dates (e.g. "11th to 13th" → 11, 12, 13).
3. **Add** one row per date to the `## Days Off` table in
   `SKILL_DIR/sprint_team_config.md`. Use the format `| Name | YYYY-MM-DD | PTO |`.
   Skip any row that already exists.
4. Proceed with the normal workflow — the Python script reads `## Days Off`
   automatically.

Examples of inline PTO phrases to recognise:
- "Henry Barboza was with PTO from the 11th to 13th"
- "David Suarez had PTO on the 7th"
- "Adrian Rojas was off on march 5 and 6"

## Workflow

### Step 0 — Read team configuration

Read `SKILL_DIR/sprint_team_config.md` and extract:
1. **Emails** — every `Email` cell in the `## Team Members` table.
2. **Jira Cloud ID** and **Project** from `## Jira Settings`.
3. Days off and holidays are handled automatically by the Python script;
   you do NOT need to parse them yourself.

### Step 1 — Resolve Jira account IDs

For **each** email, call the MCP tool `lookupJiraAccountId`:

```
server: user-atlassian
tool:   lookupJiraAccountId
args:   { "cloudId": "<jira_cloud_id>", "searchString": "<email>" }
```

Collect the `accountId` and `displayName` for every user.
Build a map: `email -> { accountId, displayName }`.

### Step 2 — Search for issues (JQL)

Build one JQL query covering all users and the sprint window.
Use `searchJiraIssuesUsingJql` (max 100 results per page; paginate if needed):

```
server: user-atlassian
tool:   searchJiraIssuesUsingJql
args:
  cloudId: "<jira_cloud_id>"
  jql: >
    project = <jira_project>
    AND issuetype in subTaskIssueTypes()
    AND assignee in (<accountId1>, <accountId2>, ...)
    AND (
      (resolutiondate >= "<sprint_start>" AND resolutiondate <= "<sprint_end>")
      OR (status in ("To Do", "In Progress") AND updated >= "<sprint_start>")
    )
    ORDER BY assignee ASC, resolutiondate DESC
  maxResults: 100
  fields:
    - key
    - summary
    - status
    - assignee
    - reporter
    - labels
    - project
    - created
    - resolutiondate
    - timeoriginalestimate
    - comment
  responseContentFormat: "markdown"
```

If `nextPageToken` is returned, call again with that token until all issues are fetched.

### Step 3 — Get changelog for each issue

For every issue key returned in Step 2, call `getJiraIssue` to obtain
transition history:

```
server: user-atlassian
tool:   getJiraIssue
args:
  cloudId: "<jira_cloud_id>"
  issueIdOrKey: "<ISSUE_KEY>"
  expand: "changelog"
  fields: ["status","assignee"]
```

From the changelog entries, extract:

| Field to extract | How to find in changelog |
|---|---|
| `started_date` | Earliest history item where `field == "status"` and `toString == "In Progress"` |
| `on_hold_date` | History item where `field == "status"` and `fromString == "In Progress"` and `toString == "To Do"` (after started_date) |
| `restarted_date` | History item where `field == "status"` and `toString == "In Progress"` AFTER `on_hold_date` |
| `assignation_date` | Earliest history item where `field == "assignee"` and the `to` value matches the current assignee |
| `assigned_by` | The `author.displayName` of the changelog entry that set the assignee |

If the changelog is empty or missing entries, fall back:
- `started_date` defaults to `created` date
- `assignation_date` defaults to `created` date
- `assigned_by` defaults to `reporter.displayName`

### Step 4 — Build the JSON data file

Create a JSON file at `SKILL_DIR/_sprint_data.json` with this structure:

```json
{
  "sprint_start": "YYYY-MM-DD",
  "sprint_end": "YYYY-MM-DD",
  "tickets": [
    {
      "finished_date": "YYYY-MM-DDTHH:MM:SS or null",
      "create_date": "YYYY-MM-DDTHH:MM:SS",
      "started_date": "YYYY-MM-DD",
      "on_hold_date": "YYYY-MM-DD or null",
      "restarted_date": "YYYY-MM-DD or null",
      "assignation_date": "YYYY-MM-DD",
      "label": "IN-SPRINT",
      "estimation_hours": 0,
      "non_workable_days": 0,
      "assigned_by": "Display Name",
      "dev": "Display Name",
      "project": "PROJECT - Name",
      "jira_ticket": "MFGR10-12345",
      "ticket_name": "Issue summary",
      "resolution_details": "",
      "status": "Done",
      "effort_comments": ""
    }
  ]
}
```

Leave `non_workable_days` as `0` — the Python script will compute it
automatically from the team config file.

Field mapping from Jira response:

| JSON field | Source |
|---|---|
| `finished_date` | `fields.resolutiondate` |
| `create_date` | `fields.created` |
| `started_date` | changelog (see Step 3) |
| `on_hold_date` | changelog (see Step 3) |
| `restarted_date` | changelog (see Step 3) |
| `assignation_date` | changelog (see Step 3) |
| `label` | first entry of `fields.labels[]`, or `""` |
| `estimation_hours` | `fields.timeoriginalestimate / 3600` (field is in seconds), or `0` |
| `non_workable_days` | `0` (auto-calculated by script) |
| `assigned_by` | changelog (see Step 3) |
| `dev` | `fields.assignee.displayName` |
| `project` | `fields.project.name` |
| `jira_ticket` | `key` |
| `ticket_name` | `fields.summary` |
| `resolution_details` | Last comment body by the assignee (from `fields.comment.comments[]`), or `""` |
| `status` | `fields.status.name` |
| `effort_comments` | `""` (manual field) |

### Step 5 — Generate the Excel report

Run the Python script **with `--team-config`** so non-workable days are
auto-calculated from the markdown:

```bash
cd SKILL_DIR
python generate_sprint_report.py \
  --data _sprint_data.json \
  --team-config sprint_team_config.md \
  --output "Sprint <M-D-YY start> ~ <M-D-YY end>.xlsx"
```

Replace `SKILL_DIR` with the absolute path to this skill's folder
(e.g. `c:/Deacom/.cursor/skills/generate-sprint-report`).

Where `<M-D-YY start>` and `<M-D-YY end>` use the format `3-1-26` (no leading zeros,
two-digit year). Example: `"Sprint 3-1-26 ~ 3-14-26.xlsx"`.

### Step 6 — Report back

Tell the user:
- The file path of the generated `.xlsx`
- A summary table: how many tickets per dev, broken down by colour
  (completed / on hold / carry over)
- Remind the user to update `sprint_team_config.md` if any days off or holidays
  need adjusting

## Error Handling

- If `lookupJiraAccountId` returns no results for an email, warn the user and
  skip that person.
- If no issues are found, tell the user and do not generate a file.
- If `openpyxl` is not installed, run `pip install openpyxl` before executing
  the script.

## Example Invocations

### Default (full team, current sprint)

> /generate-sprint-report

Agent reads `sprint_team_config.md`, uses all listed emails, auto-calculates
sprint dates.

### Override with specific emails

> /generate-sprint-report for arojas@ecisolutions.com, dsuarez@ecisolutions.com

Agent uses only those two emails. Still reads config for Jira settings and
passes `--team-config` for days-off calculation.

### Custom date range (natural language)

> /generate-sprint-report for the 2nd to 13th of march

Agent interprets as `sprint_start = 2026-03-02`, `sprint_end = 2026-03-13`.
Uses the full team list from config.

### Inline PTO

> /generate-sprint-report Henry Barboza was with PTO from the 11th to 13th

Agent adds three rows to `## Days Off` in `sprint_team_config.md`
(March 11, 12, 13 for Henry Barboza), then generates the report normally.

### Relative period

> /generate-sprint-report for only last week

Agent calculates last week's Monday–Sunday and uses those as sprint dates.

### Combined date range + PTO

> /generate-sprint-report for the 1st to 14th of march, David Suarez had PTO on the 7th

Agent sets `sprint_start = 2026-03-01`, `sprint_end = 2026-03-14`, adds
March 7 as a day off for David Suarez, then generates.
