# MCP Server – PostgreSQL (Training DB)

MCP server to talk to the **training** PostgreSQL database defined in [../docker-compose.yaml](../docker-compose.yaml) (service `training-db`).

## Prerequisites

- Node.js 18+
- PostgreSQL running (e.g. `docker compose up -d` in the repo root)

## Setup

```bash
cd mcp-postgres
npm install
npm run build
```

Optional: copy `.env.example` to `.env` and adjust if your DB is not local:

```bash
cp .env.example .env
```

Defaults match the training container: `localhost:5432`, user `training`, password `training`, database `training`.

## Tools

| Tool | Description |
|------|-------------|
| **query** | Run a **read-only** SQL query (SELECT only). Use `$1`, `$2` in SQL and pass `params` for values. |
| **list_tables** | List all tables in the `public` schema (e.g. `tnfclty`, `tnwrhse`, `tnitem`). |
| **describe_table** | Get columns, types, and nullability for a given table. |

## Run locally (stdio)

```bash
npm start
```

Or with env vars:

```bash
PG_HOST=localhost PG_PORT=5432 PG_USER=training PG_PASSWORD=training PG_DATABASE=training node dist/index.js
```

## Use in Cursor

1. **Build the server** (from this folder):

   ```bash
   npm install && npm run build
   ```

2. **Add the MCP server** in Cursor:
   - Open **Cursor Settings** → **MCP** (or **Features** → **MCP**).
   - Add a new server. Example for a **command**-based config:

   **Option A – Command (recommended)**

   - **Name:** `postgres-training` (or any name).
   - **Command:** `node`
   - **Args:** `C:\Users\adrian.rojas\OneDrive - Jalasoft\Documents\repos\deacom-training\mcp-postgres\dist\index.js`
   - **Env** (optional; defaults work if DB is on localhost:5432):
     - `PG_HOST` = `localhost`
     - `PG_PORT` = `5432`
     - `PG_USER` = `training`
     - `PG_PASSWORD` = `training`
     - `PG_DATABASE` = `training`

   **Option B – Config file**

   If Cursor uses an MCP config file (e.g. `~/.cursor/mcp.json` or project `.cursor/mcp.json`), add:

   ```json
   {
     "mcpServers": {
       "postgres-training": {
         "command": "node",
         "args": ["C:\\Users\\adrian.rojas\\OneDrive - Jalasoft\\Documents\\repos\\deacom-training\\mcp-postgres\\dist\\index.js"],
         "env": {
           "PG_HOST": "localhost",
           "PG_PORT": "5432",
           "PG_USER": "training",
           "PG_PASSWORD": "training",
           "PG_DATABASE": "training"
         }
       }
     }
   }
   ```

   Use the path to **your** `mcp-postgres` folder and `dist/index.js` if different.

3. Restart Cursor or reload MCP so it picks up the new server.

After that, you can use the MCP tools (e.g. `list_tables`, `describe_table`, `query`) from Cursor against the training database.
