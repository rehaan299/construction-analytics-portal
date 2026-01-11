ximport { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { apiUrl } from '../../services/api.service';

@Component({
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './report-form.component.html',
  styleUrl: './report-form.component.css'
})
export class ReportFormComponent {
  projectId!: number;
  reportDate = new Date().toISOString().slice(0,10);
  laborHours = 100;
  equipmentHours = 20;
  progressPercent = 5;
  notes = '';
  message = '';
  error = '';

  constructor(private http: HttpClient, route: ActivatedRoute) {
    this.projectId = Number(route.snapshot.paramMap.get('projectId'));
  }

  submit() {
    this.message = '';
    this.error = '';

    const body = {
      projectId: this.projectId,
      reportDate: this.reportDate,
      laborHours: this.laborHours,
      equipmentHours: this.equipmentHours,
      progressPercent: this.progressPercent,
      notes: this.notes
    };

    this.http.post(apiUrl('/api/reports'), body).subscribe({
      next: () => this.message = 'Report submitted successfully.',
      error: (e) => this.error = e?.error || 'Submission failed.'
    });
  }
}
