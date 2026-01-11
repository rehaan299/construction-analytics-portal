import csv
import random
from datetime import date, timedelta

random.seed(7)

PROJECTS = [
    ("ALPHA", "Project Alpha - Tower Build", date(2026,1,1), date(2026,12,31), 5_000_000),
    ("BRAVO", "Project Bravo - Logistics Yard Expansion", date(2026,2,1), date(2026,10,15), 2_200_000),
    ("CHARLIE","Project Charlie - Healthcare Retrofit", date(2026,3,10), date(2026,9,30), 1_400_000),
]

COST_CODES = ["LABOR", "EQUIPMENT", "MATERIAL_CONCRETE", "MATERIAL_STEEL", "SUBCON", "OTHER"]

def daterange(start, days):
    for i in range(days):
        yield start + timedelta(days=i)

def main(out_dir="data/sample", days=30):
    # projects.csv
    with open(f"{out_dir}/projects.csv", "w", newline="") as f:
        w = csv.writer(f)
        w.writerow(["Code","Name","StartDate","EndDatePlanned","Budget"])
        for code, name, s, e, b in PROJECTS:
            w.writerow([code, name, s.isoformat(), e.isoformat(), b])

    # daily_field_reports.csv
    with open(f"{out_dir}/daily_field_reports.csv", "w", newline="") as f:
        w = csv.writer(f)
        w.writerow(["ProjectCode","ReportDate","LaborHours","EquipmentHours","ProgressPercent","Notes","SubmittedBy"])

        for code, _, s, _, _ in PROJECTS:
            progress = 0
            for d in daterange(s, days):
                labor = random.randint(70, 140)
                equip = random.randint(10, 40)
                progress += random.choice([0, 1, 1, 2])  # slow steady progress
                progress = min(progress, 100)

                notes = random.choice([
                    "Normal production day",
                    "Weather delay",
                    "Material delivery lag",
                    "Equipment downtime",
                    "Safety stand-down (brief)"
                ])

                w.writerow([code, d.isoformat(), labor, equip, progress, notes, "field1"])

    # cost_entries.csv
    with open(f"{out_dir}/cost_entries.csv", "w", newline="") as f:
        w = csv.writer(f)
        w.writerow(["ProjectCode","CostDate","CostCode","Amount","Description","Source"])

        for code, _, s, _, _ in PROJECTS:
            for d in daterange(s, days):
                # 0-2 costs per day
                for _ in range(random.randint(0, 2)):
                    cc = random.choice(COST_CODES)
                    amount = round(random.uniform(3000, 45000), 2)
                    desc = f"{cc} entry"
                    w.writerow([code, d.isoformat(), cc, amount, desc, "ERP_SIM"])

    print("Generated synthetic CSVs in", out_dir)

if __name__ == "__main__":
    main()
