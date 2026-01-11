# Functional Specification

## 1. Authentication & Authorization
- JWT-based authentication
- Roles: FieldUser, ProjectManager, Finance, Executive, Admin
- Project scoping:
  - FieldUser: only assigned projects
  - PM: assigned + review
  - Finance/Executive: all

## 2. Field Reporting
### Submit Daily Report
Inputs:
- ProjectId
- ReportDate
- LaborHours
- EquipmentHours
- ProgressPercent (0..100)
- Notes

Validations:
- LaborHours >= 0 and <= 2000
- EquipmentHours >= 0 and <= 2000
- ProgressPercent 0..100
- ReportDate not in future

## 3. Costs & ERP Import
- CostEntries stored per project/date/category
- Admin endpoint supports importing costs (simulated ERP feed)
- ERP Sync service can be triggered manually (or scheduled in real enterprise)

## 4. Alert Rules
Rules evaluated:
- DailyCostSpike: Daily costs exceed configured threshold
- BudgetBurnRisk: (ActualCostToDate / Budget) exceeds percent threshold
- ProductivityRisk: LaborHours rising while progress stalls (simple proxy)

Alerts contain:
- ProjectId
- AlertType
- Severity (Info/Warning/Critical)
- Message
- CreatedAt
- Resolved flag

## 5. Dashboards (API KPIs)
Project KPIs endpoint returns:
- Budget
- ActualCostToDate
- BudgetUsedPercent
- LatestProgressPercent
- Last7DaysLaborHours
- Last7DaysCost
- ActiveAlertsCount

## 6. Non-functional
- Containerized deployment
- DB schema with keys and indexes
- Code test coverage for core alert logic
