import { Routes } from '@angular/router';
import { DashboardPageComponent } from './pages/dashboard/dashboard.page';
import { WorkflowsPageComponent } from './pages/workflows/workflows.page';
import { TasksPageComponent } from './pages/tasks/tasks.page';
import { AuditPageComponent } from './pages/audit/audit.page';
import { SettingsPageComponent } from './pages/settings/settings.page';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  { path: 'dashboard', component: DashboardPageComponent },
  { path: 'workflows', component: WorkflowsPageComponent },
  { path: 'tasks', component: TasksPageComponent },
  { path: 'audit', component: AuditPageComponent },
  { path: 'settings', component: SettingsPageComponent }
];
