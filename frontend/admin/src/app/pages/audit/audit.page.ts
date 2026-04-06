import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { PilotFlowApiService } from '../../core/pilotflow-api.service';
import { AuditTimelineItemResponse } from '../../core/api.models';

@Component({
  selector: 'app-audit-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './audit.page.html',
  styleUrl: './audit.page.scss'
})
export class AuditPageComponent {
  private readonly api = inject(PilotFlowApiService);

  tenantId = 'tenant-demo';
  events: AuditTimelineItemResponse[] = [];
  loading = false;
  errorMessage = '';

  ngOnInit() {
    this.loadTimeline();
  }

  loadTimeline() {
    this.loading = true;
    this.errorMessage = '';

    this.api.getAuditTimeline(this.tenantId).subscribe({
      next: (events) => {
        this.events = events;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load audit timeline.';
        this.loading = false;
      }
    });
  }
}
