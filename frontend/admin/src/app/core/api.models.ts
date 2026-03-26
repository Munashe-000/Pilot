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
