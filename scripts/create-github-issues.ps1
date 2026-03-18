<#
.SYNOPSIS
  Creates GitHub issues from user_stories.md (deacom-training backlog).

.DESCRIPTION
  Requires GITHUB_TOKEN env var (classic PAT with repo scope, or fine-grained with Issues write).
  Optionally creates labels, then POSTs each issue. Run once; duplicates if re-run.

.PARAMETER Owner
  GitHub org or user (default: AdrianRojasA).

.PARAMETER Repo
  Repository name (default: deacom-training).

.EXAMPLE
  $env:GITHUB_TOKEN = "ghp_xxx"; .\scripts\create-github-issues.ps1
#>
param(
    [string] $Owner = "AdrianRojasA",
    [string] $Repo = "deacom-training"
)

$ErrorActionPreference = "Stop"
$token = $env:GITHUB_TOKEN
if (-not $token) {
    Write-Error "Set GITHUB_TOKEN to a PAT with repo/issues access. See docs/GITHUB_SETUP.md"
}

$api = "https://api.github.com"
$headers = @{
    Authorization        = "Bearer $token"
    Accept               = "application/vnd.github+json"
    "X-GitHub-Api-Version" = "2022-11-28"
}

function Invoke-GitHubJson {
    param([string]$Method, [string]$Uri, [object]$Body = $null)
    $params = @{ Uri = $Uri; Method = $Method; Headers = $headers }
    if ($Body) {
        $json = $Body | ConvertTo-Json -Depth 10 -Compress
        $params.Body = $json
        $params.ContentType = "application/json; charset=utf-8"
    }
    return Invoke-RestMethod @params
}

function Ensure-Label {
    param([string]$Name, [string]$Color, [string]$Description)
    $labelsUri = "$api/repos/$Owner/$Repo/labels/$([uri]::EscapeDataString($Name))"
    try {
        Invoke-WebRequest -Uri $labelsUri -Headers $headers -Method Head -UseBasicParsing | Out-Null
    } catch {
        $null = Invoke-GitHubJson -Method Post -Uri "$api/repos/$Owner/$Repo/labels" -Body @{
            name        = $Name
            color       = $Color
            description = $Description
        }
        Write-Host "Created label: $Name"
    }
}

foreach ($l in @(
    @{ n = "type-question"; c = "5319E7"; d = "Primary contact / explanation" },
    @{ n = "type-bug"; c = "D73A4A"; d = "Defect" },
    @{ n = "type-feature"; c = "0E8A16"; d = "Feature" },
    @{ n = "optional"; c = "FBCA04"; d = "Optional / bonus" }
)) { Ensure-Label -Name $l.n -Color $l.c -Description $l.d }

$issues = @(
    @{
        title  = "[Q-001] Primary Contact: why Login before Item/Entry?"
        labels = @("type-question")
        body   = @"
## User story
The Primary Contact asks why `/Login` must be called before `/Item/Entry`, and why `/Item/Entry` alone errors.

## Steps to reproduce
1. On `main`, call `/Item/Entry` first (increase quantity for an item in a facility) — **error**.
2. Call `/Login`, then `/Item/Entry` — **success**.

## Sample body

``````
```json
{
  "itemCode": "PANT-001",
  "destinationType": 0,
  "destinationId": 1,
  "additionalQuantity": 1000,
  "description": ""
}
```
``````

## Dev task
Explain current behavior to **Rodrigo Alarcón** (Teams message; bonus if in English). **No code change required.**
"@
    },
    @{
        title  = "[D-001] Cannot add or update Warehouse (POST/PUT errors)"
        labels = @("type-bug")
        body   = @"
## User story
Errors when inserting or updating a warehouse via Post/Put Warehouse endpoints (different errors per method).

## Expected
Warehouses should insert/update without errors.
"@
    },
    @{
        title  = "[D-002] Cannot increase item quantity in warehouse (POST /Item/Increase)"
        labels = @("type-bug")
        body   = @"
## User story
`POST /Item/Increase` returns an error; cannot increase quantity in a warehouse.

## Expected
User can increase quantity in warehouses or facilities.
"@
    },
    @{
        title  = "[F-003] Warehouse reporting endpoint (qty > 0 only)"
        labels = @("type-feature")
        body   = @"
## User story
Report state of a given warehouse: all items with quantities, **only items with quantity > 0**.

## Expected
New endpoint for warehouse reporting; return only rows with quantity more than 0.
"@
    },
    @{
        title  = "[D-003] Get all facilities returns empty / wrong data"
        labels = @("type-bug")
        body   = @"
## User story
Get All Facilities endpoint returns empty object.

## Expected
Should return correct facility data.
"@
    },
    @{
        title  = "[D-004] Disallow negative inventory"
        labels = @("type-bug")
        body   = @"
## Expected
Inventory must not allow negative values.
"@
    },
    @{
        title  = "[F-004] Inventory outcomes from Warehouse and Facilities"
        labels = @("type-feature")
        body   = @"
## Expected
Support outcomes (removals/consumption) of items from warehouses and facilities.
"@
    },
    @{
        title  = "[F-005] Inventory movements between Warehouses and Facilities"
        labels = @("type-feature")
        body   = @"
## User story
As a user, move inventory between warehouses and facilities (either direction).

## Expected
Support movements between warehouses and facilities.
"@
    },
    @{
        title  = "[F-006] Offices (storage tiers: Large/Medium/Small)"
        labels = @("type-feature")
        body   = @"
## User story
Support **Offices** as storage like warehouses/facilities, with caps:
- **Large**: 200 items  
- **Medium**: 125 items  
- **Small**: 50 items  

Future: offices as sole place for Sales Orders (see F-011).

## Dev notes
- Table ``tnoffc`` — Office  
- Table ``tnitmofc`` — Items ↔ Offices link  
"@
    },
    @{
        title  = "[F-011] Sales Orders (offices only, DB persistence)"
        labels = @("type-feature")
        body   = @"
## User story
Sales Orders only from **Offices**; persist all SO data for later calculations.

## Expected
- SO reduces office inventory; if insufficient stock, clear message.  
- Custom numeration: ``so_number`` distinct from PK.  
- Table ``tnssord`` — Sales Orders  
- Table ``tnitprce`` — item prices; default **USD 15** if no price  
"@
    },
    @{
        title  = "[F-012] Sell pants endpoint (SO + warehouse→office transfer)"
        labels = @("type-feature")
        body   = @"
## User story
Endpoint to sell pants at a specific facility: list of items + quantities; respect F-011/F-006 rules.

## Expected
- Create Sales Order; return **SO number** and **total price**.  
- Message when office has insufficient stock.  
- **Bonus:** transfer from warehouse to office when needed.  
"@
    },
    @{
        title  = "[F-001] (Optional) Simple error feedback instead of raw exception dump"
        labels = @("type-feature", "optional")
        body   = @"
## User story
On errors, return a simple, readable message instead of a large technical payload.

## Expected
Clear error message; explain illegal action when useful.
"@
    }
)

$created = 0
foreach ($i in $issues) {
    $payload = @{ title = $i.title; body = $i.body; labels = $i.labels }
    $r = Invoke-GitHubJson -Method Post -Uri "$api/repos/$Owner/$Repo/issues" -Body $payload
    Write-Host "Created #$($r.number): $($r.title)"
    $created++
}

Write-Host "`nDone. Created $created issues. Add them to GitHub Project **deacom-training** (Projects tab)."
exit 0
