Set up the read-only SQL Server MCP for a specific worktree by reading that worktree's `deacom.ini` and writing values to `.cursor/mcp.json`.

Steps:
1. Ask me which worktree folder to use (examples: `1703`, `1704`, `DeacNet`).
2. Run:
   `powershell -ExecutionPolicy Bypass -File "c:\Deacom\tools\mysql-readonly-mcp\configure-mcp-from-ini.ps1" -WorkspaceRoot "c:\Deacom" -Worktree "<WORKTREE>"`
3. The script must:
   - Read `SERVER`, `DATABASE`, and related path/url values from `<WORKTREE>\deacom.ini`
   - Use SQL Server defaults from Deacom conventions (`Microsoft.Data.SqlClient` behavior)
   - Use Windows credentials by default (`-AuthMode windows`) unless I provide `-AuthMode sql` and `-SqlUser`
   - Prompt for SQL password only in SQL auth mode
   - Replace `.cursor/mcp.json` with the generated MCP configuration
4. After writing config, tell me to restart Cursor so MCP reconnects.
