import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PilotFlowApiService } from '../../core/pilotflow-api.service';
import { DecideTaskRequest, TaskInboxItemResponse } from '../../core/api.models';

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
  decidedBy = 'Security Lead';

  tasks: TaskInboxItemResponse[] = [];
  loading = false;
  errorMessage = '';
  decisionMessage = '';
  decisionError = '';
  decisionNotes: Record<string, string> = {};
  decisionInFlight: Record<string, boolean> = {};

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

  canDecide(task: TaskInboxItemResponse) {
    return task.status !== 'Completed';
  }

  submitDecision(task: TaskInboxItemResponse, decision: 'Approved' | 'Rejected') {
    if (!this.canDecide(task) || this.decisionInFlight[task.id]) {
      return;
    }

    this.decisionMessage = '';
    this.decisionError = '';
    this.decisionInFlight[task.id] = true;

    const note = this.decisionNotes[task.id]?.trim();
    const payload: DecideTaskRequest = {
      tenantId: this.tenantId,
      decision,
      decidedBy: this.decidedBy,
      comment: note ? note : undefined
    };

    this.api.decideTask(task.id, payload).subscribe({
      next: () => {
        this.decisionMessage = `${decision} recorded for ${task.requesterName}.`;
        this.decisionNotes[task.id] = '';
        this.loadTasks();
        this.decisionInFlight[task.id] = false;
      },
      error: (error) => {
        this.decisionError = error?.error?.message || 'Unable to record decision.';
        this.decisionInFlight[task.id] = false;
      }
    });
  }
}
