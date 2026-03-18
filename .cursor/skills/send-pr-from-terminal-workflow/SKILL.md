---
name: send-pr-from-terminal-workflow
description: Prepares and sends a pull request using a ticket-based git workflow. Use when the user provides a folder, a list of files to commit, and a ticket code like MFGR10-41713, or asks to create a branch, commit selected files, push, and open a PR.
---

# Send PR From Terminal Workflow

## Inputs

Collect these required inputs before running commands:

- `folder`: repository folder to operate in (example: `C:/Deacom/1703`)
- `files`: explicit list of files to stage and commit
- `ticket_code`: ticket like `MFGR10-41713`

Optional:

- `commit_message_detail`: one sentence describing the behavior change
- `base_branch`: target branch for PR (default `svn/DeacNet1703000` unless user says otherwise)
- `author_alias`: branch prefix username (default inferred from current branch or user handle)

## Workflow

1. Validate that `folder`, `files`, and `ticket_code` are present.
2. Switch into `folder`.
3. Create or switch to branch: `<author_alias>/<ticket_code>`.
4. Stage only the requested files:
   - Run one `git add` command with the provided file paths.
   - Do not stage unrelated files.
5. Commit with subject format:
   - `<ticket_code>: <commit_message_detail>`
   - If `commit_message_detail` is missing, ask for it.
6. Push branch and set upstream:
   - `git push -u origin HEAD`
7. Create PR with title:
   - `<ticket_code>: <short summary>`
8. Return PR URL plus a compact list of committed files.

## Safety Rules

- Never commit files not listed in `files`.
- Never use destructive git operations (`reset --hard`, force push) unless explicitly requested.
- If generated files are included by mistake, stop and ask for confirmation.
- If commit fails due to hooks, resolve and retry with a new commit.

## Command Template

Use these commands as a template (replace placeholders):

```bash
cd "<folder>"
git checkout -b "<author_alias>/<ticket_code>"
git add "<file1>" "<file2>"
git commit -m "<ticket_code>: <commit_message_detail>"
git push -u origin HEAD
gh pr create --base "<base_branch>" --title "<ticket_code>: <short summary>" --body "$(cat <<'EOF'
## Summary
- <change 1>
- <change 2>

## Test Plan
- [ ] <test 1>
- [ ] <test 2>
EOF
)"
```

## Example Invocation

Input:

- `folder`: `C:/Deacom/1703`
- `files`: `["Framework/Deacom.BusinessClasses/Job.cs"]`
- `ticket_code`: `MFGR10-41713`
- `commit_message_detail`: `When MRP creates jobs for FG items with sub-assemblies, job lines use parent-level required quantity instead of an absolute exploded target.`

Expected branch:

- `arojas/MFGR10-41713`

Expected commit subject:

- `MFGR10-41713: When MRP creates jobs for FG items with sub-assemblies, job lines use parent-level required quantity instead of an absolute exploded target.`
