import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProjectsComponent } from './components/projects/projects.component';
import { ReportFormComponent } from './components/report-form/report-form.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'projects', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'projects', component: ProjectsComponent, canActivate: [authGuard] },
  { path: 'report/:projectId', component: ReportFormComponent, canActivate: [authGuard] },
  { path: 'dashboard/:projectId', component: DashboardComponent, canActivate: [authGuard] }
];
