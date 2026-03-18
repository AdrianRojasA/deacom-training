#!/usr/bin/env node

import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import {
  CallToolRequestSchema,
  ListToolsRequestSchema,
} from "@modelcontextprotocol/sdk/types.js";
import pg from "pg";

// Connection config: matches docker-compose.yaml (training-db)
const config = {
  host: process.env.PG_HOST ?? "localhost",
  port: parseInt(process.env.PG_PORT ?? "5432", 10),
  user: process.env.PG_USER ?? "training",
  password: process.env.PG_PASSWORD ?? "training",
  database: process.env.PG_DATABASE ?? "training",
};

const pool = new pg.Pool(
  process.env.DATABASE_URL
    ? { connectionString: process.env.DATABASE_URL }
    : config
);

const server = new Server(
  {
    name: "mcp-postgres-training",
    version: "1.0.0",
  },
  {
    capabilities: {
      tools: {},
    },
  }
);

server.setRequestHandler(ListToolsRequestSchema, async () => ({
  tools: [
    {
      name: "query",
      description:
        "Execute a read-only SQL query (SELECT) against the training database. Use for inspecting data, listing rows, or analytics.",
      inputSchema: {
        type: "object" as const,
        properties: {
          sql: {
            type: "string",
            description: "A read-only SQL query (SELECT only). Use $1, $2 for parameters.",
          },
          params: {
            type: "array",
            description: "Optional query parameters for $1, $2, etc.",
            items: { type: "string" },
          },
        },
        required: ["sql"],
      },
    },
    {
      name: "list_tables",
      description:
        "List all tables in the public schema of the training database (e.g. tnfclty, tnwrhse, tnitem).",
      inputSchema: {
        type: "object" as const,
        properties: {},
      },
    },
    {
      name: "describe_table",
      description:
        "Get column names, data types, and nullable info for a given table.",
      inputSchema: {
        type: "object" as const,
        properties: {
          table: {
            type: "string",
            description: "Table name (e.g. tnitem, tnfclty, tnwrhse).",
          },
        },
        required: ["table"],
      },
    },
  ],
}));

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  const { name, arguments: args } = request.params;
  const safeArgs = (args ?? {}) as Record<string, unknown>;

  try {
    if (name === "query") {
      const sql = String(safeArgs.sql ?? "").trim();
      if (!sql.toLowerCase().startsWith("select")) {
        return {
          content: [
            {
              type: "text" as const,
              text: JSON.stringify({
                error: "Only SELECT queries are allowed for safety.",
              }),
            },
          ],
          isError: true,
        };
      }
      const params = Array.isArray(safeArgs.params) ? safeArgs.params : [];
      const client = await pool.connect();
      try {
        const result = await client.query(sql, params);
        return {
          content: [
            {
              type: "text" as const,
              text: JSON.stringify(
                { rows: result.rows, rowCount: result.rowCount ?? result.rows.length },
                null,
                2
              ),
            },
          ],
        };
      } finally {
        client.release();
      }
    }

    if (name === "list_tables") {
      const client = await pool.connect();
      try {
        const result = await client.query(
          `SELECT table_name FROM information_schema.tables
           WHERE table_schema = 'public' AND table_type = 'BASE TABLE'
           ORDER BY table_name`
        );
        return {
          content: [
            {
              type: "text" as const,
              text: JSON.stringify(
                result.rows.map((r) => r.table_name),
                null,
                2
              ),
            },
          ],
        };
      } finally {
        client.release();
      }
    }

    if (name === "describe_table") {
      const table = String(safeArgs.table ?? "").trim();
      if (!table) {
        return {
          content: [{ type: "text" as const, text: "Missing required argument: table" }],
          isError: true,
        };
      }
      const client = await pool.connect();
      try {
        const result = await client.query(
          `SELECT column_name, data_type, is_nullable
           FROM information_schema.columns
           WHERE table_schema = 'public' AND table_name = $1
           ORDER BY ordinal_position`,
          [table]
        );
        if (result.rows.length === 0) {
          return {
            content: [
              {
                type: "text" as const,
                text: JSON.stringify({
                  error: `Table '${table}' not found in public schema.`,
                }),
              },
            ],
            isError: true,
          };
        }
        return {
          content: [
            {
              type: "text" as const,
              text: JSON.stringify(result.rows, null, 2),
            },
          ],
        };
      } finally {
        client.release();
      }
    }

    return {
      content: [{ type: "text" as const, text: `Unknown tool: ${name}` }],
      isError: true,
    };
  } catch (err) {
    const message = err instanceof Error ? err.message : String(err);
    return {
      content: [
        {
          type: "text" as const,
          text: JSON.stringify({ error: message }),
        },
      ],
      isError: true,
    };
  }
});

async function main() {
  const transport = new StdioServerTransport();
  await server.connect(transport);
}

main().catch((err) => {
  console.error(err);
  process.exit(1);
});
