# Exercise 5: Optimize the Full Inventory Report

**Time limit:** 30 minutes

## Scenario

Operations exports a complete inventory report from:

`GET /api/Report/FullInventory`

The report is correct. It returns every item with its type, the quantity held in
each warehouse and facility, the item's total quantity, and the item's share of
all stock of the same type. The business is happy with the content, but the
report has become slow as the inventory has grown, and it is getting worse.

Each response includes two diagnostic headers:

- `X-Database-Query-Count`: how many SQL commands the report executed
- `Server-Timing`: how long the report took to build, in milliseconds

## Task

Use AI to find out why the report is slow, and make it fast without changing
what it returns.

1. Call the endpoint and record a baseline before changing anything.
2. Find the causes of the slowness and back each one with evidence. Look at both
   the C# code and the SQL it runs.
3. Rank what you find by impact and explain your plan before editing.
4. Fix the issues in order of impact. Keep each change focused.
5. Prove the report still returns the same data.
6. Summarize what you fixed, the before-and-after numbers, and what you would do
   next with more time.

There is more than one problem. Finding and fixing the biggest ones well is
better than touching everything superficially.

## Rules

- The JSON response must not change: same fields, values, and ordering.
- The report must stay complete. Pagination, caching, or removing data does not
  count as a fix.
- Do not run many small queries in parallel to hide the cost.
- Keep the changes inside the report controller, service, and DTOs. Schema or
  index changes are allowed only with `EXPLAIN` evidence, delivered as a
  separate SQL script.
- The database is PostgreSQL. `psql` is available through
  `docker exec -it postgres psql -U training -d training`.

## Evidence to Show at the End

- Query count and timing before and after
- How you confirmed that the output did not change
- The `EXPLAIN` or `EXPLAIN ANALYZE` output behind any SQL-level finding

Timing varies between machines. The query count and identical output are the
most reliable proof.

## Working Style

AI use is expected. Treat AI suggestions as hypotheses: check that a suggested
cause really costs time, read the SQL it generates, and verify the result
yourself.
