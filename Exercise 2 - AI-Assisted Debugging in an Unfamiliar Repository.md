# Exercise 2: AI-Assisted Debugging in an Unfamiliar Repository

**Duration:** 30 minutes (setup is done before the clock starts)  
**Format:** Live debugging with your AI assistant. Talk aloud while you work.

---

## The Bug Report

> "Every time the API restarts, adding stock with `POST /Item/Entry` fails with a 500 error. Someone found that calling `POST /api/Login` first makes it work, so we've been doing that as a workaround. Entry shouldn't depend on Login."

Request body that reproduces it:

```json
{
  "itemCode": "PANT-001",
  "destinationType": 0,
  "destinationId": 1,
  "additionalQuantity": 5,
  "description": ""
}
```

`destinationType: 0` is a facility, and `destinationId: 1` is the "Central NY Store" facility.

---

## Deliverables

1. **Root cause:** Why Entry fails before Login and works after it
2. **Minimal fix:** A targeted change that removes the dependency on Login
3. **Proof:** Entry works on a freshly started API without calling Login, and the quantity changes in the database
4. **Verbal close:** Defect, root cause, fix, proof, and one remaining risk

---

## Steps

### 1. Reproduce (5 min)

1. Restart the API (`Ctrl+C`, then `dotnet run`).
2. In Swagger, call `POST /Item/Entry` with the body above. Note the status code and the error.
3. Call `POST /api/Login`, then repeat the Entry call. Note what changed.

### 2. Hypothesize and trace (8 min)

- Ask your AI assistant to trace both requests from the controller to the database.
- Read the code it points to and confirm it yourself.
- **Before changing anything**, state your hypothesis aloud: what Login sets up, and why Entry needs it.

### 3. Fix (10 min)

- Ask for the **smallest** change that removes the dependency.
- If the suggestion rewrites more than the bug needs, push back.
- Review the diff before you run it.

### 4. Prove it and close (7 min)

1. Restart the API so nothing is left in memory from earlier calls.
2. Call `POST /Item/Entry` **without** calling Login. Confirm it returns 200.
3. Confirm the quantity went up:

   ```bash
   docker exec postgres psql -U training -d training -c "SELECT if_quantity FROM tnitmflty WHERE if_fcid = 1 AND if_itid = 12;"
   ```

4. Close aloud:

   ```
   Defect:         ...
   Root cause:     ...
   Fix:            ...
   Proof:          ...
   Remaining risk: ...
   ```

**Stretch, if time remains:** Send the same request with `"destinationType": 1` (warehouse). What happens, and why?

---

## Before the Clock Starts

1. Start the database from the repo root:

   ```bash
   docker compose up -d
   ```

2. Start the API:

   ```bash
   cd src/DeacomTraining
   dotnet run
   ```

3. Open Swagger at https://localhost:7038/swagger.
4. Open your AI assistant on the repo.

---

## Time

| Step | Time |
|------|------|
| Reproduce | 5 min |
| Hypothesize and trace | 8 min |
| Fix | 10 min |
| Prove it and close | 7 min |
| **Total** | **30 min** |
