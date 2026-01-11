import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { apiUrl } from '../../services/api.service';

@Component({
  standalone: true,
  imports: [RouterLink],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent {
  projects: any[] = [];
  loading = true;

  constructor(private http: HttpClient) {
    this.http.get<any[]>(apiUrl('/api/projects')).subscribe(p => {
      this.projects = p;
      this.loading = false;
    });
  }
}
