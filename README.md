# Integrated Construction Analytics & Workflow Portal (Enterprise Construction Prototype)

> A deployable internal portal prototype that connects **field reporting** with **corporate visibility** (cost, labor, progress), designed for **enterprise construction companies (e.g., PCL Construction and peers)**.
>
> ⚠️ This is a **portfolio prototype** using **synthetic data** and **simulated ERP feeds**. It is **not affiliated with or endorsed by any company**.

---

## Why this exists (business problem)

Large construction enterprises run complex operations across:
- **Site crews** (foremen, superintendents, engineers) entering daily production reality
- **Corporate staff** (PMs, finance, cost control, operations leadership) needing timely visibility

In many real environments, progress and cost data is fragmented across spreadsheets, manual reporting, and legacy ERP modules. That delay causes:
- late intervention on overruns
- duplicate data entry
- weak real-time decision making

**Goal:** Provide **one internal system** that captures daily site signals and makes them actionable through:
- a central SQL database
- integration-style ERP sync (simulated)
- workflow automation (alerts)
- dashboards and KPI endpoints

---

## What you get in this repo

### ✅ Full-stack system (deployable)
- **Backend:** ASP.NET Core 8 Web API (JWT auth, role-based access, EF Core)
- **Database:** SQL Server (container) with schema + seed
- **Frontend:** Angular portal UI (login, projects, daily report form, dashboard)
- **Automation:** PowerShell “AlertRunner” (stand-in for Power Automate)
- **Docs:** BRD, functional spec, use cases, UAT plan, training guide
- **Architecture diagrams:** Mermaid source files
- **Synthetic data scripts:** generator + importer

---

## High-level architecture

**Data flow:**
1) Foreman submits daily report (hours, progress, notes) via Angular UI  
2) API writes to SQL Server (single source of truth)  
3) ERP sync service (simulated) imports budgets/cost codes/cost entries  
4) Alert rules evaluate thresholds (budget burn, daily cost spikes, schedule risk proxies)  
5) Corporate users see dashboards / alerts immediately

See diagrams in:
- `architecture/System_Architecture.mmd`
- `architecture/Data_Model_ERD.mmd`
- `architecture/Process_Flow_Workflow.mmd`

---

## Roles (RBAC)

This prototype supports enterprise role separation:

- **FieldUser** (Foreman/Superintendent): submit reports, view their project(s)
- **ProjectManager**: view project dashboards, review reports, see alerts
- **Finance**: view all projects, costs, budget health, alerts
- **Executive**: portfolio view (aggregate)
- **Admin**: seed/manage users (prototype)

---

## Quickstart (Docker)

### 1) Prereqs
- Docker Desktop
- (Optional for local dev) Node 20+, .NET 8 SDK

### 2) Configure environment
Copy `.env.example` to `.env`:

```bash
cp .env.example .env
