# Test Implemented Feature

**Project is already built.** Do not run `dotnet build` unless asked. Revert any build-prop changes after tests.

1. **Identify the feature** from the user's last commit, prompt, or ticket. If unclear, ask for the file, method, or ticket ID.

2. **Determine the folder** (user may say "in @1704"). Default to the folder from recent context.

3. **Find related tests** in `<folder>\Quality\Deacom.Quality` and `<folder>\Quality\Deacom.UnitTests`. Search by test name, category, or references to the changed types/files. List what you find (test name + source file).

4. **Tell the user to run the tests in Visual Studio Test Explorer** (Quality projects are .NET Framework 4.8). Provide the test names to run.

5. **Analyze failures.** Accept input as:
   - A **test log path** (e.g. `C:\Users\...\VsTempFiles\<TestName>-<id>.testlog`) — read the file for stack trace, standard output, and failure point.
   - A **screenshot** — extract the test name, error message, and stack trace lines.
   - **Pasted text** — parse the error and stack trace directly.

6. **Classify and fix.** Walk the stack trace from innermost frame outward:

   | Failure type | Signals | Action |
   |---|---|---|
   | **Setup / config** | `OneTimeSetUp`, `InvalidConnectionStringException`, `Login failed` | Don't edit code. Tell user to fix `deacom.ini`, `keyfile.txt`, or SQL Server login. |
   | **Empty grid / null row** | `NullReferenceException` on `FirstOrDefault().GetString(...)`, `BaseCursor` empty | Check if a prefilter is missing (e.g. `FormValues["partnumber"]` before `ViewClick()`). Check if the grid query excludes rows (INNER JOIN that should be LEFT JOIN). **Use Debug mode** to inspect the SQL query and cursor contents if the cause is unclear. |
   | **Missing column** | `ArgumentException: Column 'X' does not belong to table` | Add `HasColumn("X")` guard before access; derive value from available data if column absent. |
   | **Null value** | `SqlNullValueException`, `ArgumentNullException` on `.Value` | Add `!value.IsNull` check before `.Value` on `SqlDateTime` or similar. |
   | **Assertion mismatch** | `Assert.AreEqual` / `DeacomAssert` failure | Compare expected vs actual; trace the data source. **Use Debug mode** if the data flow is non-obvious. |

   **When to use Debug mode:** Switch to Debug mode when (a) the root cause isn't clear from the stack trace alone, (b) you need to trace data through multiple methods or cursors, or (c) multiple layers (test setup → form logic → grid query) could each be the source.

   **Output format:**
   - **≤ 5 failures:** 2 lines each — (1) one-line summary (exception + cause); (2) one-line fix.
   - **> 5 failures:** General solution in 2–4 lines.

7. If all tests pass: "All related tests passed."

**Reference:** `ProdTickets/feb1-feb14-26/Unit Testing Manual - 1703.md`
