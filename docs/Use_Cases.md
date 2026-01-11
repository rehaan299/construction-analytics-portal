# Use Cases

## UC-01 Submit Daily Report
Actor: FieldUser  
Preconditions: Logged in, assigned project exists  
Main Flow:
1. FieldUser opens Report Form
2. Enters hours/progress/notes
3. Submits
4. System validates and stores report
5. System evaluates alert rules (optional immediate)
Postcondition: Report stored and visible to corporate dashboards

## UC-02 View Project Dashboard
Actor: ProjectManager / Finance / Executive  
Main Flow:
1. User selects project
2. System returns KPI snapshot + trends + active alerts
3. User filters by date range (future enhancement)

## UC-03 Import Costs (ERP Simulation)
Actor: Admin  
Main Flow:
1. Admin triggers cost import endpoint
2. System stores new CostEntries
3. System evaluates alert rules and creates alerts if needed

## UC-04 Automated Alert Runner (Power Automate stand-in)
Actor: Automation  
Main Flow:
1. Scheduled job calls /api/alerts/evaluate
2. Alerts created/updated
3. Notifications sent (or logged in demo)
