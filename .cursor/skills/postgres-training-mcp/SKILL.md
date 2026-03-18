---
name: postgres-training-mcp
description: >-
  Uses the postgres-training MCP server for the training PostgreSQL database.
  Use when the user mentions database tables, rows, columns, schema, SQL
  queries, tnfclty, tnwrhse, tnitem, or any training DB data. Do not infer
  schema or row counts from repo SQL files alone—call MCP tools first.
---

# Postgres training database (MCP)

## Rule

Whenever the prompt involves **tables, rows, columns, counts, sample data, joins, or schema** for the **training** Postgres DB (docker-compose `training-db`), **use the MCP server `postgres-training`** before answering. Do not rely on memory, guesses, or only on static `.sql` files in the repo.

## Server and tools

| Tool | When to use |
|------|-------------|
| **list_tables** | Discover tables in `public` (e.g. before naming tables). |
| **describe_table** | Columns, types, nullability for a named table. |
| **query** | Read-only `SELECT` for row counts, filters, aggregates, samples. Use `$1`, `$2` with `params` for bound values. |

## Workflow

1. If tables are unknown → **list_tables**.
2. If columns or types matter → **describe_table** for each relevant table.
3. If the user asks for data, counts, or examples → **query** with appropriate SQL.

## Scope

- Database: **`training`** (user `training`, typical tables include `tnfclty`, `tnwrhse`, `tnitem`, etc.).
- **query** is **SELECT-only**; do not attempt writes via MCP.

## Anti-patterns

- Answering “what columns does X have?” without **describe_table**.
- Answering “how many rows?” without **query**.
- Assuming init SQL in `setup/` is the only source of truth—data may differ from scripts.
