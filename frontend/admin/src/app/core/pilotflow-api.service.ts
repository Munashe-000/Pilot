import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from './api.constants';
import {
  CreateAccessRequestRequest,
  CreateAccessRequestResponse,
  TaskInboxItemResponse
} from './api.models';

@Injectable({ providedIn: 'root' })
export class PilotFlowApiService {
  private readonly http = inject(HttpClient);

  createAccessRequest(payload: CreateAccessRequestRequest) {
    return this.http.post<CreateAccessRequestResponse>(
      `${API_BASE_URL}/api/access-requests`,
      payload
    );
  }

  getTaskInbox(tenantId: string, assigneeRole: string) {
    return this.http.get<TaskInboxItemResponse[]>(
      `${API_BASE_URL}/api/tasks/inbox`,
      {
        params: { tenantId, assigneeRole }
      }
    );
  }
}
