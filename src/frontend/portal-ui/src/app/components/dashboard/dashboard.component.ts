import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { apiUrl } from '../../services/api.service';

@Component({
  standalone: true,
  imports: [RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  projectId!: number;
  kpis: any = null;
  alerts: any[] = [];
  loading = true;

  constructor(private http: HttpClient, route: ActivatedRoute) {
    this.projectId = Number(route.snapshot.paramMap.get('projectId'));
    this.load();
  }

  load() {
    this.loading = true;

    this.http.get<any>(apiUrl(`/api/projects/${this.projectId}/kpis`)).subscribe(k => {
      this.kpis = k;
      this.loading = false;
    });

    this.http.get<any[]>(apiUrl(`/api/alerts?projectId=${this.projectId}`)).subscribe(a => {
      this.alerts = a;
    });
  }

  badgeClass(sev: string) {
    if (sev === 'Critical') return 'badge badge-critical';
    if (sev === 'Warning') return 'badge badge-warning';
    return 'badge badge-info';
  }
}
