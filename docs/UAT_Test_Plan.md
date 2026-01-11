# User Acceptance Testing (UAT) Plan

## Scope
Validate that the portal supports key workflows for field and corporate users.

## Entry Criteria
- Database seeded
- API and UI running via Docker
- Demo users available

## Test Scenarios (sample)
| ID | Scenario | Steps | Expected Result |
|---|---|---|---|
| UAT-01 | Field submits valid report | Login field1 → submit report | Success message, report appears in list |
| UAT-02 | Validation blocks bad hours | Submit negative hours | UI shows validation error |
| UAT-03 | PM sees updated KPIs | Login pm1 → dashboard | KPIs reflect latest report |
| UAT-04 | Costs import triggers alert | Admin import high cost | Alert created with Warning/Critical |
| UAT-05 | RBAC access control | field1 tries view all projects | Only assigned projects visible |

## Exit Criteria
- All Critical scenarios pass
- Known issues documented
