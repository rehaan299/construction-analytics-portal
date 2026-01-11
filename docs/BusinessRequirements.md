# Business Requirements Document (BRD)
## Integrated Construction Analytics & Workflow Portal

### 1. Background
Enterprise construction operations involve coordination between:
- Field crews submitting daily production reality (hours, progress, issues)
- Corporate staff monitoring budgets, cost health, schedule adherence

Data is often fragmented across spreadsheets and ERP modules, leading to delayed visibility.

### 2. Problem Statement
“How might we develop an internal system that connects construction field data with corporate enterprise systems in near real time, providing both site crews and office staff with unified, actionable insights?”

### 3. Objectives
- Centralize daily field reporting and project KPIs into a single data source
- Reduce reporting latency and duplicate entry
- Trigger automated alerts for exceptions (cost spikes, budget burn risk)
- Enable dashboards for PM/Finance/Executive stakeholders

### 4. In Scope
- Daily field report capture (labor hours, equipment hours, progress %, notes)
- Central SQL database with referential integrity
- ERP integration simulation (budget + costs import)
- Alert evaluation & notifications (stand-in automation)
- Role-based access control
- KPI endpoints consumable by BI (Power BI)

### 5. Out of Scope (prototype)
- Real SSO integration (Azure AD)
- Photo/document storage
- Mobile offline mode
- Real ERP connections

### 6. Stakeholders / Personas
- Site Foreman (FieldUser)
- Project Manager
- Finance / Cost Controller
- Executive / Ops Manager
- Admin (IT)

### 7. High-Level Requirements
BR-01: Field users can submit daily reports for their assigned projects  
BR-02: Corporate users can view dashboards with near real-time KPIs  
BR-03: Costs and budgets can be imported from ERP-like feed (simulated)  
BR-04: System evaluates exception rules and generates alerts  
BR-05: RBAC restricts access by role and project assignment  

### 8. Success Metrics
- Daily reporting captured consistently (demo dataset shows daily entries)
- Alerts generated within minutes of threshold breach (automation runnable)
- Dashboard KPIs queryable via API in < 1s for demo volume
