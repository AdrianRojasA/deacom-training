# Exercise 1: AI-Assisted PR Review

**Duration:** 20 minutes  
**Format:** Live PR review with your AI assistant. Talk aloud while you work.

---

## The Scenario

A teammate opened a PR on this branch (`feature/sloppy-architecture-review`) that "refactors items and facilities". It touches:

- `src/DeacomTraining/Controllers/ItemController.cs`
- `src/DeacomTraining/Service/ItemService.cs`
- `src/DeacomTraining/Controllers/FacilityController.cs`
- `src/DeacomTraining/Repositories/ItemRepository.cs` (new file)

You are the reviewer. Decide whether it can merge.

---

## Deliverables

1. **Decision:** Approve, Approve with comments, or Request changes
2. **Blockers:** The issues that stop the merge, each with file, what is wrong, and why it matters
3. **One PR comment:** Written as you would post it to the author

---

## Steps

### 1. Check that it builds (3 min)

```bash
cd src/DeacomTraining
dotnet build
```

Note the result before reading any code.

### 2. Understand the change (5 min)

```bash
git diff main...HEAD --stat -- src
git diff main...HEAD -- src
```

- Ask your AI assistant to summarize what the PR changes.
- Compare the PR with how the existing code is layered. For example, `WarehouseController` calls `WarehouseService`, and the service reaches the database through `SqlExecute` and `Cursor`.
- Check the summary against the diff yourself. Don't take it on trust.

### 3. Sort the findings (7 min)

Split what you find into two lists:

- **Blockers:** Must be fixed before merge
- **Follow-ups:** Worth raising, but they should not hold the PR

Areas to check:

- Does it build and run?
- Does it remove or rename anything existing clients call?
- How is SQL built from request data?
- Does each layer do its own job?
- Does the new file fit the existing patterns, and is it used?

### 4. Decide and comment (5 min)

- Make the call and explain it in one or two sentences.
- Write the most important PR comment: what is wrong, why, and what you want changed.

---

## References in the Repo

- Existing pattern: `Controllers/WarehouseController.cs` and `Service/WarehouseService.cs`
- Database schema: `setup/init.pg.sql`
- Code standards: `.cursor/rules/csharp-naming-and-helper-standards.mdc`

---

## Time

| Step | Time |
|------|------|
| Check that it builds | 3 min |
| Understand the change | 5 min |
| Sort the findings | 7 min |
| Decide and comment | 5 min |
| **Total** | **20 min** |
