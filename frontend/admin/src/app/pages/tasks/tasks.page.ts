import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PilotFlowApiService } from '../../core/pilotflow-api.service';
import { TaskInboxItemResponse } from '../../core/api.models';

@Component({
  selector: 'app-tasks-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tasks.page.html',
  styleUrl: './tasks.page.scss'
})
export class TasksPageComponent {
  private readonly api = inject(PilotFlowApiService);

  tenantId = 'tenant-demo';
  assigneeRole = 'Security';

  tasks: TaskInboxItemResponse[] = [];
  loading = false;
  errorMessage = '';

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.loading = true;
    this.errorMessage = '';

    this.api.getTaskInbox(this.tenantId, this.assigneeRole).subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load tasks. Please try again.';
        this.loading = false;
      }
    });
  }

  statusClass(status: string) {
    switch (status) {
      case 'Escalated':
        return 'status alert';
      case 'Completed':
        return 'status ok';
      case 'InProgress':
        return 'status warn';
      default:
        return 'status warn';
    }
  }

  priorityLabel(priority: string) {
    switch (priority) {
      case 'High':
        return 'High priority';
      case 'Low':
        return 'Low priority';
      default:
        return 'Normal priority';
    }
  }
}
