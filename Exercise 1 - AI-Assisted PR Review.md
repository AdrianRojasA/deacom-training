# Exercise 1: AI-Assisted PR Review & Architectural Decisions
## DeacomTraining Clean Architecture Review

**Duration:** 15 minutes  
**Format:** Rapid timed PR review with AI assistance  
**Objective:** Quickly identify architectural violations and make informed decisions about what's critical vs. what can wait.

---

## 🎯 The Scenario

A developer submitted a PR attempting to refactor the DeacomTraining project. The PR includes:
- Changes to `Controllers/ItemController.cs`
- Changes to `Service/ItemService.cs`  
- Changes to `Controllers/FacilityController.cs`
- A NEW `Repositories/ItemRepository.cs` file

**Your Job:** Rapidly assess whether these changes align with the existing architecture (defined in CLAUDE.md). Identify what's critical to block and what can be discussed or fixed later.

---

## 📋 Your Task (15 minutes)

**Deliver:**
1. **Blocker Assessment** — Which files/changes cannot merge in their current form? (Why?)
2. **Conversation Starters** — 1-2 key PR comments on the most critical issues
3. **Go/No-Go Decision** — Can this PR proceed with feedback, or must it be rejected?

---

## ⚡ Rapid Workflow

### Step 1: Baseline (2 minutes)
- Get familiar with the project and find information about the architecture of what each layer should do

### Step 2: Scan All Changed Files (3 minutes)
Look for:
- Logic changes
- Wrong patterns
- New patterns that weren't approved

### Step 3: Focus on Blockers (7 minutes)
Examine the files and look for any critical issues:
- **ItemController.cs** - What should be in this layer?
- **ItemRepository.cs** - What should be in this layer?
- **Security** - Is there any security concerns?

For each blocker, note:
- **File and line(s)**
- **What's wrong**
- **Why it's critical** (references CLAUDE.md or standards)

### Step 4: Write 1-2 Key Comments (2 minutes)
Draft the most important PR comment (not all of them):


### Step 5: Make the Call (1 minute)
- **Request changes** if there are blockers
- **Approve with comments** if it's just architectural style/learning
- Explain your reasoning to the interviewer


---

## ✅ What We're Evaluating

| Criterion | What We're Looking For |
|-----------|------------------------|
| **Speed** | Can you make good architectural decisions under time pressure? |
| **Pattern Recognition** | Do you quickly spot what violates the existing architecture? |
| **Critical Thinking** | Do you distinguish blockers from style issues? |
| **Communication** | Can you articulate why something is a blocker in 1-2 sentences? |
| **Judgment** | Do you defer to standards or make nuanced decisions? |
| **Pragmatism** | Do you understand which issues kill a PR vs. which can iterate? |

---

## 🔗 Key References

- **Architecture:** `CLAUDE.md` — Each layer's responsibilities
- **Current Patterns:** `Controllers/WarehouseController.cs`, `Service/WarehouseService.cs`
- **Naming:** `.cursor/rules/csharp-naming-and-helper-standards.mdc`

---

## 🚀 Getting Started

```bash
cd c:\Users\adrian.rojas\repos\deacom-training
git checkout feature/sloppy-architecture-review
git diff main..feature/sloppy-architecture-review --stat  # See what changed
git diff main..feature/sloppy-architecture-review | head -200  # Scan first changes
```

---

## 📝 Your Deliverable

At the end of 15 minutes, be ready to tell the interviewer:

1. **Can this PR merge?** (Yes / Request Changes)
2. **What's the blocker?** (1-2 sentence explanation)
3. **What would you tell the developer?** (1-2 key PR comments)
4. **Why?** (Reference the architectural standard)

## ⏱️ Time Allocation

- Baseline: 2 min
- Scan diffs: 3 min
- Focus on blockers: 7 min
- Write key comments: 2 min
- Make the call: 1 min

---

## 🎤 During the Exercise

- **Work aloud** — talk through your reasoning
- **Use AI for quick clarification** — *"Is this controller pattern correct per CLAUDE.md?"*
- **Stay focused** — don't get lost in minor style issues
- **Make a call** — Request Changes or Approve with Comments, with clear reasoning
