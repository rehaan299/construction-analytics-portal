import csv
import requests

API = "http://localhost:8080"
ADMIN_USER = "admin"
ADMIN_PASS = "Password123!"

def login():
    r = requests.post(f"{API}/api/auth/login", json={"username": ADMIN_USER, "password": ADMIN_PASS})
    r.raise_for_status()
    return r.json()["token"]

def get_projects(token):
    r = requests.get(f"{API}/api/projects", headers={"Authorization": f"Bearer {token}"})
    r.raise_for_status()
    return {p["code"]: p["id"] for p in r.json()}

def import_costs(token, csv_path):
    proj_map = get_projects(token)
    payload = []

    with open(csv_path, newline="") as f:
        reader = csv.DictReader(f)
        for row in reader:
            pid = proj_map[row["ProjectCode"]]
            payload.append({
                "projectId": pid,
                "costDate": row["CostDate"],
                "costCode": row["CostCode"],
                "amount": float(row["Amount"]),
                "description": row["Description"],
                "source": row.get("Source","ERP_SIM")
            })

    r = requests.post(f"{API}/api/costs/import", json=payload, headers={"Authorization": f"Bearer {token}"})
    r.raise_for_status()
    print(r.json())

if __name__ == "__main__":
    token = login()
    import_costs(token, "data/sample/cost_entries.csv")
