# GitHub setup: repo + project + issues

## 1. Repo on GitHub (already configured)

This repository is already linked to GitHub:

| Item | Value |
|------|--------|
| **Remote** | `origin` |
| **URL** | `https://github.com/AdrianRojasA/deacom-training.git` |
| **Branch** | `main` (tracks `origin/main`) |

If you need the code under **your** account instead:

1. Create a new empty repo on GitHub (e.g. `your-user/deacom-training`).
2. Update remote:  
   `git remote set-url origin https://github.com/YOUR_USER/deacom-training.git`
3. Push:  
   `git push -u origin main`

---

## 2. Create the GitHub Project **deacom-training**

1. Open your GitHub user:  
   [github.com/AdrianRojasA](https://github.com/AdrianRojasA).
2. Click **Projects** → **New project**.
3. Choose a template (e.g. **Team backlog** or **Table**).
4. Name the project **`deacom-training`**.
5. Link the project to the **`deacom-training`** repository when prompted (or add the repo in project settings).
6. After issues exist (step 3), add them: open each issue → **Projects** in the sidebar → add to **deacom-training**, or from the project use **Add item** → **From repository** and select issues.

---

## 3. Create issues from `user_stories.md`

### Option A — Script (recommended)

1. Create a [Personal Access Token](https://github.com/settings/tokens) (classic) with scope **`repo`** (or **Issues** write for the repo only, if using fine-grained token).
2. In PowerShell:

   ```powershell
   $env:GITHUB_TOKEN = "ghp_xxxxxxxx"   # your token
   cd "path\to\deacom-training"
   .\scripts\create-github-issues.ps1
   ```

3. Optional: use another owner/repo:

   ```powershell
   .\scripts\create-github-issues.ps1 -Owner "your-user" -Repo "deacom-training"
   ```

The script creates labels if missing, then opens one issue per user story (Q-001, D-001, … F-012, optional F-001).

### Option B — GitHub CLI

Install [GitHub CLI](https://cli.github.com/), run `gh auth login`, then you can create issues manually or adapt the bodies from `user_stories.md`.

---

## 4. Commit message convention

From `user_stories.md`: put the ticket id first, e.g.  
`git commit -m "F-001 Added custom exception for unknown errors"`
