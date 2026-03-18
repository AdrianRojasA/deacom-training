---
description: Command – Generate a SOLUTION.md summary for a ticket (issue, behavior, fix, code changes, PD/QA, journal of approaches that didn't work)
alwaysApply: false
---
# Generate SOLUTION Summary (Command)

When the user asks to **generate a SOLUTION summary** (e.g. "Generate a MFGR10-41713-SOLUTION.md summary" or "make solution summary for ticket X"), produce a markdown file named **`<TICKET>-SOLUTION.md`** (e.g. `MFGR10-41713-SOLUTION.md`) with this structure.

## Required structure

### Section 1 – Short summary (2 lines each)
- **Original issue:** 2 lines describing the reported problem.
- **Actual behavior found:** 2 lines describing what was found in code/logs/reproduction.
- **Solution summary:** 2 lines describing the fix and outcome.

### Section 2 – Code changes and PD/QA
- **Code changes:** Summary of code changes with **file paths and approximate line/location** (e.g. `Job.cs ~4831–4844`, `AddSubAssemblies`).
- **PD/QA:** If found in chat – questions asked to Product Design or QA and their answers (brief).

### Section 3 – Journal (approaches that did not solve the issue)
- If found in chat: brief **journal-style** summary of solution approaches and findings that **did not** fix the issue (e.g. "Adjusting only X did not fix it because Y"; "Assuming Z was the cause was rejected by logs").

## Usage
- Place the file in the same folder as the ticket/plan (e.g. `ProdTickets/feb1-feb14-26/`).
- Use ticket/plan/LORE and conversation context to fill each section accurately.
- Keep each "2 line" block to exactly 2 lines where specified.

## Final Step - PR Handoff (Optional)
- After generating `<TICKET>-SOLUTION.md`, append a PR handoff block (outside the solution file body) with:
  - `ticket_code`: `<TICKET>`
  - `folder`: `<repo/worktree folder>`
  - `files`: `[<changed file paths only>]`
  - `commit_message_detail`: `<one-sentence behavior/fix summary>`
  - `base_branch`: `<target base branch if known>`
- Ask for confirmation: "PR handoff prepared. Execute PR flow now?"
- If confirmed, invoke the `send-pr-from-terminal-workflow` skill with those inputs.

## Example trigger phrases
- "Generate a MFGR10-41713-SOLUTION.md summary with..."
- "Create SOLUTION.md for ticket X..."
