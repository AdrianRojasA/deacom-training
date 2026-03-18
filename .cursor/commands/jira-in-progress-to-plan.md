---
description: Command – Fetch in-progress Jira subtask(s) assigned to you, create ProdTickets markdown, then generate a plan
alwaysApply: false
---
# Jira In-Progress → ProdTicket Markdown → Plan

Automate the flow: **Jira in-progress subtask → ProdTickets markdown file → plan-for-ticket**.

## Step 1: Fetch in-progress subtasks from Jira

1. Call `getAccessibleAtlassianResources` to get the cloud ID.
2. Call `atlassianUserInfo` to get the current user.
3. Search Jira with JQL:
   ```
   assignee = currentUser() AND issuetype = Sub-task AND status in ("To Do", "In Progress") ORDER BY updated DESC
   ```
4. If no subtasks are found, inform the user and stop.
5. If multiple subtasks exist, ask which one to process, or process the first (most recently updated) by default.
6. After the ticket is selected, rename the current agent chat to `<KEY> - <keyword>` where `<KEY>` is the Jira issue key (e.g. `MFGR10-44095`) and `<keyword>` is a short 2-3 word phrase from the ticket summary (e.g. `MFGR10-44095 - QC Copy Error`).

## Step 2: Create ProdTickets markdown from the ticket

For the selected subtask (or the first one):

1. Fetch full issue details via `getJiraIssue` (cloudId, issueIdOrKey). Also fetch the **parent ticket** details to retrieve migration/version info.
2. Determine the **Affected Version** by checking these sources (in priority order):
   - **"Migration Details" section** on the subtask or parent ticket. Look for a block like `Migration Details` → `Affected Version: XX.XX.XXX.XXXX` (e.g. `17.04.009.0004`). This is the most common location.
   - Version pattern in the subtask or parent summary (regex: `\d{2}\.\d{2}\.\d{3}\.\d{4}`)
   - Jira `fixVersions` or `versions` fields on the subtask or parent ticket
   - If none found, leave as "Unknown — confirm with ticket reporter"
3. Determine the sprint folder for today's date:
   - Sprint folders are 2-week periods: `monthdd-monthdd-yy` (e.g. `feb15-feb28-26`).
   - Create the folder if it does not exist.
4. Create a markdown file with filename: `Dev 1 - <KEY> - <summary>.md` (or `Exec 1 - ...` if the parent is an Exec-type ticket).
5. Populate the markdown with this structure:

```markdown
# Dev 1 - <KEY> - <summary>

**Parent:** [<parent-key>](<jira-url>) – <parent-summary>
**Project:** <project-name> (<project-key>)
**Status:** <status>
**Labels:** <labels>
**Assignee:** <assignee-displayName>
**Created:** <created-date>
**Updated:** <updated-date>

---

## Affected Version: <resolved from Jira fixVersions, versions, summary, or description>

<version string, e.g. 17.04.009.0004>

---

## Description

<full Jira description, preserving formatting>

---

## Setup Steps

<extract from description if present>

---

## Steps to Reproduce

<extract from description if present>

---

## Execution

<extract from description if present>

---

## Verification / Additional Notes

<extract from description if present>

---

## Considerations

<related issues, recommendations, links from description>

---

## Dev Template (to fill)

Resolution:

Dev Changes:

Dev Validations:
```

6. Save the file to `ProdTickets/<sprint-folder>/Dev 1 - <KEY> - <sanitized-summary>.md`.

## Step 3: Invoke plan-for-ticket command

1. **Infer the worktree folder** from the Affected Version resolved in Step 2:
   - Extract the major.minor from the version string (e.g. `17.04.009.0004` → `17.04` → folder `1704`; `17.03.010.0003` → `1703`).
   - Strip the dot between major and minor: `17.04` → `1704`, `17.03` → `1703`.
   - Verify the folder exists in the workspace root (e.g. `c:\Deacom\1704\`).
   - If the version is unknown, the folder doesn't exist, or the mapping is ambiguous, **ask the user** which worktree folder to use for the code analysis.
2. Pass the created markdown file path **and the inferred worktree folder** to the **plan-for-ticket** command.
3. Tell the user: "Created `ProdTickets/<sprint>/<filename>.md` from Jira <KEY>. Affected Version: `<version>` → analyzing code in `<folder>/`. Proceeding with plan generation..."
4. Execute the plan-for-ticket command using the created file as input, restricting code analysis to the inferred worktree folder.

## Usage

- Run via `/jira-in-progress-to-plan` or by asking to "create a plan from my in-progress Jira ticket".
- Optionally specify a ticket key: "create a plan from MFGR10-42865" – then fetch that specific ticket instead of searching.
- Optionally specify a worktree folder for the plan output.

## Metadata for plan-for-ticket

The created markdown provides:
- **Title:** From Jira summary
- **Description:** Full Jira description with setup, steps to reproduce, execution, verification
- **Affected version:** Affected version or version from Migration details section
- **Metadata:** Parent, project, status, labels, assignee, dates
- **Considerations:** Related issues, recommendations, environment notes
