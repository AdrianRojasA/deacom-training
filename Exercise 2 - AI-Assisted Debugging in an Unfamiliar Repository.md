# Exercise 2: AI-Assisted Debugging in an Unfamiliar Repository
## DeacomTraining Warehouse Management API

**Duration:** 30 minutes (environment setup is done before the clock starts)  
**Format:** Live practical exercise with verbal reasoning  
**Objective:** Investigate and fix a defect in an unfamiliar repository using AI-assisted tools and critical thinking.

---

## 🎯 The Problem Scenario

**User Report:**
> "We've noticed inconsistent behavior in our warehouse management system. When we create a new warehouse using `POST /warehouse`, it appears to succeed and returns a 200 response with warehouse data. However, when we immediately call `GET /warehouse` to list all warehouses, the newly created warehouse is missing from the response. But if we call `/login` again and then `GET /warehouse`, the new warehouse appears. This is happening randomly—not every time, but frequently enough to be a real issue."

**Additional Context:**
- The DeacomTraining project is an ASP.NET Core 6.0 inventory management API
- It supports two database backends: SQL Server and PostgreSQL
- Users can switch between backends via configuration
- The team is currently using **PostgreSQL** via Docker Compose
- There is no in-memory cache or session state documented in the public API contract

---

## 📋 Your Task

In 30 minutes, investigate this defect and deliver:
1. **Root cause** — Explain *why* this behavior occurs
2. **A minimal fix** — Implement a targeted change that resolves the issue without refactoring unrelated code
3. **One regression check** — A test or a repeated manual call that shows create-then-list now returns the new warehouse
4. **Verbal close** — Defect, root cause, what you changed, and one remaining risk

---

## 🔍 Suggested Workflow

### Step 1: Orient and hypothesize (8 minutes)
- Ask your AI assistant to map the related paths
- Read the files the map points to and confirm them yourself
- **Before making any changes**, state your hypothesis aloud:
  - *Example:* "I think `POST /warehouse` writes to the database, but `GET /warehouse` reads an in-memory list that `/login` fills. That would explain why the warehouse shows up only after logging in again."

### Step 2: Confirm the root cause (7 minutes)
- Check whether an in-memory collection exists, when it is filled, and whether create updates it
- Reproduce once in Swagger or a REST client:
  1. `POST /login`
  2. `POST /warehouse` with a new name
  3. `GET /warehouse` — note whether the new warehouse is missing
  4. `POST /login`, then `GET /warehouse` again
- Say whether the reproduction confirms or changes your hypothesis

### Step 3: Minimal fix (10 minutes)
- Ask for a **minimal** change that updates the in-memory list when a warehouse is created, and that still works for SQL Server and PostgreSQL
- If the suggestion rewrites unrelated files push back and ask for a smaller change
- Implement that change yourself and review the diff before moving on

### Step 4: One check and close (5 minutes)
- Prove the fix with one check: create a warehouse, then list warehouses immediately, and confirm it appears
- Close aloud:
  ```
  Defect: Newly created warehouses missing from GET /warehouse until re-login
  Root cause: [why]
  Fix: [what you changed]
  Check: [what you ran and what it showed]
  Remaining risk: [one edge case you did not cover]
  ```

---

## ✅ What We're Evaluating

| Criterion | What We're Looking For |
|-----------|------------------------|
| **Onboarding Speed** | Do you get a tight map of the three relevant paths before reading widely? |
| **Quality of Questions** | Are prompts specific and constrained, or broad? |
| **Hypothesis-Driven Thinking** | Do you state a hypothesis before changing code, then adjust it from evidence? |
| **Critical Review** | Do you push back when a suggestion is larger than the bug? |
| **Time Management** | Do you stay on the minimal fix inside 30 minutes? |
| **Root-Cause Validation** | Can you explain *why* the bug occurs? |
| **Verification** | Do you show create-then-list works after the change? |
| **Communication** | Is the verbal close clear? |
| **Ownership** | Do you review and own the change, or paste the first AI diff? |

---

## 🛠️ Before the Clock Starts

Have this ready so the 30 minutes are spent on the defect:

1. **Repo:**
   ```bash
   cd c:\Users\adrian.rojas\repos\deacom-training
   ```

2. **Database** (if not already running):
   ```bash
   docker-compose up -d  # Start PostgreSQL and pgAdmin4
   ```

3. **API:**
   ```bash
   cd src/DeacomTraining
   dotnet build
   dotnet run
   ```

4. **API access:**
   - Swagger UI: https://localhost:5001/swagger
   - Base URL: https://localhost:5001/api

5. **AI assistant** open (Cursor, Claude, or equivalent)

---


## 🎤 During the Exercise

- **Talk aloud** — hypotheses, what you are checking, and why you accept or reject a suggestion
- **Show your prompts** — the interviewer should see how you steer the agent
- **Time-box** — if a step is stuck, say what you know and move to the next one. Do not spend more than 8 minutes on any single step
- **Stop at 30 minutes** — a clear root cause and a reviewed minimal fix matter more than extra tests

---

## ⏱️ Time Allocation

| Step | Time |
|------|------|
| Orient and hypothesize | 8 min |
| Confirm the root cause | 7 min |
| Minimal fix | 10 min |
| One check and verbal close | 5 min |
| **Total** | **30 min** |

---

## 🚀 Ready?

When the environment is up, tell your interviewer and start Step 1. Good luck!
