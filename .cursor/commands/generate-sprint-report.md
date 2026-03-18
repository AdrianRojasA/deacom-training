---
description: Generate a bi-weekly sprint report Excel from Jira. Trigger phrases include "sprint report", "generate sprint report", "create sprint report".
globs: 
alwaysApply: false
---

# Generate Sprint Report Command

When the user asks to generate a sprint report, follow the **generate-sprint-report** skill located at `.cursor/skills/generate-sprint-report/SKILL.md`.

## Quick reference

1. **Read** `.cursor/skills/generate-sprint-report/sprint_team_config.md` for the team roster, days off, holidays, and Jira settings.
2. If the user provides specific emails, use only those. Otherwise use the full team list from the config.
3. Look up Jira account IDs, run JQL search, fetch changelogs.
4. Write `_sprint_data.json` in the skill folder and run:

```bash
cd .cursor/skills/generate-sprint-report
python generate_sprint_report.py --data _sprint_data.json --team-config sprint_team_config.md --output "Sprint <start> ~ <end>.xlsx"
```

5. Report the file path and per-dev summary to the user.

## Inline features

The command supports natural-language modifiers in the same invocation:

- **Custom date range**: interpret human-friendly dates relative to the current year.
- **Inline PTO / days off**: add the mentioned days to the `## Days Off` table in `sprint_team_config.md` before running the script. Match the person's name against the team roster.
- **Relative periods**: "last week" = previous Monday–Sunday, "this week" = current Monday–today, etc.

## Examples

### Default (full team, current sprint)

> /generate-sprint-report

Uses all team members from config. Auto-calculates the current 2-week sprint window.

### Custom date range

> /generate-sprint-report for the 2nd to 13th of march

Interpret as `sprint_start = 2026-03-02`, `sprint_end = 2026-03-13` (current year).
Uses the full team list from config.

### Inline PTO

> /generate-sprint-report Henry Barboza was with PTO from the 11th to 13th

Before generating the report:
1. Parse the PTO: person = "Henry Barboza", dates = 11th–13th of the current month.
2. Add one row per date to `## Days Off` in `sprint_team_config.md` (skip if already present).
3. Then generate the report normally — the script will pick up the new days off.

### Relative period

> /generate-sprint-report for only last week

"Last week" = previous Monday through Sunday. Set `sprint_start` and `sprint_end` accordingly.

### Combined

> /generate-sprint-report for the 1st to 14th of march, David Suarez had PTO on the 7th

Parse both the date range and the PTO. Add the day off to config, then generate with the given dates.

## Maintaining the team config

Remind the user they can edit `.cursor/skills/generate-sprint-report/sprint_team_config.md` to:
- Add or remove team members (emails)
- Log individual days off (PTO, sick days) under `## Days Off`
- Add team-wide holidays under `## Team Holidays`

These are automatically subtracted as "Non Workable Days" in the report.
