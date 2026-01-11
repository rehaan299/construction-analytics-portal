# Workflow Automation (Stand-in for Power Automate)

In a real enterprise Microsoft ecosystem, this would be implemented as:
- Power Automate flow triggered on:
  - new DailyFieldReport submitted OR
  - scheduled hourly evaluation

Actions:
- evaluate alert rules
- notify project stakeholders (email / Teams)
- create task in PM system (optional)

This repo includes `AlertRunner.ps1` to simulate the same.
