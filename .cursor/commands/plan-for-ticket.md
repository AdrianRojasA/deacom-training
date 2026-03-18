Generate a **plan** for tackling a production ticket. The input is: 
1. A markdown file (usually from `ProdTickets/`) that has a title and description of the task and maybe additional findings with results of manual testing.
2. The project folder (optional), usually since the current root path is a git worktree root user will specify the worktree folder to work on.

## Output: Plan structure

Produce a plan document that aligns with the patterns used in `ProdTickets/` PLAN and Solution Report markdown files. Include:

1. **Issue summary**
   - Ticket ID and short problem statement.
   - Symptoms / observed behavior.
   - Steps to reproduce (if the ticket describes them).
   - Additional findings: renamed as "Considerations"

2. **Current behavior analysis**
   - How the code behaves today (flow, key conditions, data used).
   - Short (maximum 4 blocks) mermaid.js chart of the flow
   - Short class diagram (maximum 4 blocks) mermaid.js chart of the involved classes in the code, with labels such as forms, filters, grids, DBO, BusinessClasses
   - Root cause or likely cause if already identified.

3. **Areas to review**
   - **Specific files and line ranges** (e.g. `1703/Framework/Deacom.Common/BaseFunctions.cs` lines ~3914–3954).
   - For each area: what to look for, current code flow, and why it matters.
   - Code snippets only where they clarify the flow or bug.

4. **Areas to investigate** (when cause is unclear)
   - Concrete questions (e.g. “Is `pu_qship` actually 0 for the unreceived line?”).
   - Investigation steps and which files/methods to check.

5. **Proposed solution**
   - High-level approach (what to change and why).
   - Steps with locations (file, method, line ranges).
   - Optional: before/after code or logic description so implementation can later match a Solution Report.

6. **Verification and follow-up**
   - SQL queries or data checks to confirm behavior or root cause (when relevant).
   - Testing recommendations (what to run, what to compare).
   - Ask any clarifying questions (repro steps, env, SQL, config) that would improve the plan.

## Guidance

- **Be specific:** Prefer exact paths and line ranges (or “~lines X–Y”) over vague “in the PO module.”
- **Reuse ProdTickets style:** Use the same sectioning and level of detail as existing `PLAN - MFGR10-*.md` and `SOLUTION - MFGR10-*.md`.md` files in `ProdTickets/`.
- **Plan for a solution doc:** Write so the plan can later be turned into a Solution Report (problem summary, root cause, change details, files modified, testing).
- **Branch/version:** When relevant, note which branch or version (e.g. 1703 vs 1704) the plan targets and any porting steps.
- **Location:** Create the plan file in the correct folder corresponding to the date the command is executed. Also move the prodticket to the correct date folder if not already in. These folders are 2 week sprints, so if no folder match current date create a folder with name current date-2 weeks later-year in the format monthdd-monthdd-yy for example "nov23-dec6-25"

## Focus
1. Most markdown tickets will have its filename start as Dev 1, these are defects identified that must be solved with code fix.
2. Some of them will have its filename start with Exec 1, these ones need to be treated as research tickets, to wether confirm with product design if this is an expected behavior or if it may need a code fix. So focus on the questions to ask and how the business logic behaves for the feature involved