# Exercise Expectations & Evaluation Criteria
## Training Program - October 2026

---

## 📌 Purpose

This document defines clear expectations, success criteria, and evaluation guidelines for exercises used in trainee development. It ensures consistent assessment across all candidates and provides transparency on what constitutes "passing" an exercise.

---

## Exercise 5: Optimize the Full Inventory Report

### Learning Objectives
- Demonstrate systematic debugging methodology
- Identify performance bottlenecks using evidence (query count, timing)
- Apply optimization techniques (N+1 query elimination, eager loading, etc.)
- Explain SQL execution plans and their impact
- Validate fixes without changing business logic
- Communicate findings clearly to stakeholders

### Success Criteria

#### ✅ Passing (Mid-Level Ready)

1. **Baseline Establishment** (Required)
   - Calls endpoint and records initial query count and timing
   - Documents baseline as starting point for comparison
   - Example: "Before: 50 queries, 2500ms"

2. **Root Cause Analysis** (Required - Multiple Issues)
   - Identifies at least 2 distinct performance problems
   - Each problem backed by evidence (query logs, execution plans, code review)
   - Shows understanding of why each problem causes slowness
   - Ranks issues by impact with justification

3. **Implementation** (Required)
   - Fixes issues in priority order
   - Each change is focused and testable independently
   - No shortcuts: no pagination hacks, no data removal, no parallel queries to hide cost
   - Changes stay within allowed boundaries (controller, service, DTOs, or SQL schema with EXPLAIN evidence)

4. **Validation** (Required)
   - Proves output is identical (same fields, values, ordering)
   - Shows before/after metrics: query count reduction, timing improvement
   - Demonstrates reliability of the fix (runs multiple times, consistent results)

5. **Communication** (Required)
   - Clear summary of what was fixed
   - Before/after comparison with specific numbers
   - Next steps if more time were available
   - Technical language appropriate for stakeholders

#### ⚠️ Partial Credit (Needs Development)

- Finds 1 problem with weak evidence
- Implementation works but misses optimization opportunities
- Validation is partial or unclear
- Communication lacks specific metrics or technical depth

#### ❌ Not Passing (Needs Rework)

- No clear baseline or metrics
- Multiple problems identified but without evidence
- Fixes change the output or use disallowed shortcuts
- No attempt to validate the result
- Communication is unclear or missing

### Evaluation Rubric

| Criteria | Weight | Excellent | Good | Needs Work | Missing |
|----------|--------|-----------|------|-----------|---------|
| **Baseline & Metrics** | 15% | Clear baseline, precise numbers | Baseline present, metrics clear | Vague metrics, missing timing | No baseline |
| **Root Cause Analysis** | 30% | 2+ issues with SQL evidence, ranked by impact | 2 issues identified, mostly clear | 1 issue found, weak evidence | No analysis |
| **Implementation Quality** | 25% | All fixes focused, no shortcuts, complete optimization | Fixes work, some shortcuts considered | Works but suboptimal approach | Incomplete or wrong fixes |
| **Validation** | 20% | Output proven identical, metrics show clear improvement, reproducible | Output checked, improvement shown | Partial validation, unclear proof | No validation |
| **Communication** | 10% | Clear summary, specific numbers, next steps, appropriate technical level | Summary present, numbers included | Vague summary, missing details | Unclear or missing |

### For Luis: Focus Areas

**Primary**: Systematic debugging approach before jumping to fixes
- Don't assume what the problem is
- Gather evidence first
- Then prioritize based on impact

**Secondary**: Clear communication of technical findings
- Be specific with metrics
- Explain SQL execution plans simply
- Suggest next steps

### For Manuel: Focus Areas

**Primary**: Independent investigation without looking for "the answer"
- Use tools (query logs, EXPLAIN) properly
- Validate assumptions with evidence
- Rank issues objectively

**Secondary**: Precise implementation
- Understand why each fix works
- Don't use parallel queries to hide cost
- Explain trade-offs in your solution

---

## General Exercise Expectations

### Required for All Trainees

1. **Use AI Appropriately**
   - Treat AI suggestions as hypotheses, not answers
   - Verify every suggestion with code review
   - Check SQL execution plans
   - Prove the result yourself

2. **Document Everything**
   - Show baseline metrics before changes
   - Document reasoning for each fix
   - Prove changes don't break business logic
   - Include before/after evidence

3. **Think Systematically**
   - Break problems into smaller tasks
   - Solve in priority order
   - Validate each step
   - Communicate clearly

4. **Ask for Clarification**
   - Don't assume requirements
   - Confirm success criteria upfront
   - Ask about constraints and limitations
   - Validate understanding before starting

### Common Mistakes to Avoid

❌ **Not Reading Requirements Completely**
- Thoroughly read all task descriptions before starting
- Identify constraints (rules, allowed changes, time limits)
- Clarify success criteria with mentor

❌ **Creating Tests Without Clear Criteria**
- Define what "passing" means BEFORE writing tests
- Specify limits (query count: 10, time limit: 2000ms)
- Document criteria in writing

❌ **Jumping to Code Without Analysis**
- Analyze the problem first
- Gather evidence
- Plan the approach
- THEN implement

❌ **Not Validating Results**
- Don't assume your fix works
- Test multiple times
- Compare before/after objectively
- Use tools to verify (MCP, sqlcmd, curl, etc.)

---

## Debugging Methodology (Expected Skills)

### Level 1: Junior
- Can execute code and run tests
- Can read error messages
- Can ask for help appropriately

### Level 2: Mid-Level (Luis & Manuel Target)
- **Can systematically investigate**: Uses logs, query traces, profiling
- **Can form hypotheses**: Based on evidence, not guessing
- **Can validate fixes**: With objective metrics
- **Can explain reasoning**: Clear communication of findings

### Level 3: Senior
- Can design debugging strategy upfront
- Can mentor others through debugging
- Can identify systemic issues proactively
- Can optimize for complex scenarios

---

## Success Stories

### ✅ Good Approach (Luis/Manuel Model)

1. Call endpoint, record baseline: "50 queries, 2500ms"
2. Analyze code, find N+1 query pattern
3. Check SQL logs to confirm
4. Plan fix: "Add .Include() for relationships"
5. Implement change
6. Validate: "22 queries, 800ms, output identical"
7. Report: "Fixed N+1 queries, 56% reduction in query count, 68% time improvement"

### ❌ Poor Approach (To Avoid)

1. Read problem
2. "This is probably slow queries"
3. Add .Include() everywhere
4. Run once, seems faster
5. "Done!" - Submit without metrics or validation

---

## Assessment Guidelines for Mentors

### When Approving an Exercise

- ✅ Trainee provided objective before-and-after metrics
- ✅ Fixes are evidenced-based, not guesses
- ✅ Output validation is clear (identical data, same fields)
- ✅ Communication explains the reasoning
- ✅ No rule violations (pagination hacks, parallel queries, data removal)

### When Requesting Rework

- ❌ Metrics are missing or vague
- ❌ Root causes identified but not backed by evidence
- ❌ Fixes change the output or violate constraints
- ❌ No validation or validation is weak
- ❌ Communication is unclear or missing details

### When Moving to Next Exercise

- Both metrics and reasoning are clear
- Trainee demonstrated understanding, not just results
- They can explain what they did and why
- They can predict impact of similar problems

---

## For This Cohort: Luis & Manuel

### Immediate Focus

**Both**: 
- Establish baseline metrics consistently
- Use tools to gather evidence (not just guesses)
- Validate independently, not just by running code
- Document findings clearly

**Luis**:
- Break down problems into smaller steps
- Don't rush to coding
- Take time to structure thinking

**Manuel**:
- Use debugging tools properly (MCP, sqlcmd, curl, bash)
- Validate each hypothesis with evidence
- Explain your reasoning as you go

---

**Document Created**: October 13, 2026  
**Applies To**: Luis Fernando Arce Torrez, Manuel Alay Larrea  
**Approved By**: Adrian Rojas, Adhemar Botelo, Claudia Torrico
