INSERT INTO Projects (Code, Name, StartDate, EndDatePlanned, Budget)
VALUES
('ALPHA', 'Project Alpha - Tower Build', '2026-01-01', '2026-12-31', 5000000.00),
('BRAVO', 'Project Bravo - Logistics Yard Expansion', '2026-02-01', '2026-10-15', 2200000.00),
('CHARLIE', 'Project Charlie - Healthcare Retrofit', '2026-03-10', '2026-09-30', 1400000.00);

INSERT INTO CostEntries (ProjectId, CostDate, CostCode, Amount, Description, Source)
VALUES
(1, '2026-01-05', 'LABOR', 18000.00, 'Initial labor mobilization', 'ERP_SIM'),
(1, '2026-01-06', 'MATERIAL_CONCRETE', 42000.00, 'Concrete pour batch 1', 'ERP_SIM'),
(2, '2026-02-03', 'EQUIPMENT', 9500.00, 'Excavator rental', 'ERP_SIM'),
(3, '2026-03-15', 'LABOR', 12000.00, 'Retrofit crew start', 'ERP_SIM');

INSERT INTO DailyFieldReports (ProjectId, ReportDate, LaborHours, EquipmentHours, ProgressPercent, Notes, SubmittedBy)
VALUES
(1, '2026-01-05', 96, 18, 2, 'Site setup + safety orientation', 'field1'),
(1, '2026-01-06', 120, 22, 3, 'Concrete prep; minor weather delay', 'field1'),
(2, '2026-02-03', 80, 35, 1, 'Excavation started', 'field1');
