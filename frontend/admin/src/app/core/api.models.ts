export interface CreateAccessRequestRequest {
  tenantId: string;
  requesterName: string;
  requesterEmail: string;
  systemName: string;
  accessLevel: string;
  reason: string;
  managerName: string;
}

export interface CreateAccessRequestResponse {
  requestId: string;
  taskId: string;
  status: string;
  createdAtUtc: string;
}

export interface TaskInboxItemResponse {
  id: string;
  requestId: string;
  title: string;
  requesterName: string;
  systemName: string;
  assignedToRole: string;
  status: string;
  priority: string;
  createdAtUtc: string;
  dueAtUtc: string;
}

export interface DecideTaskRequest {
  tenantId: string;
  decision: string;
  decidedBy: string;
  comment?: string;
}

export interface DecideTaskResponse {
  taskId: string;
  requestId: string;
  taskStatus: string;
  requestStatus: string;
  decision: string;
  decidedAtUtc: string;
}

export interface AuditTimelineItemResponse {
  id: string;
  taskId: string;
  requestId: string;
  decision: string;
  actor: string;
  comment?: string;
  occurredAtUtc: string;
}
