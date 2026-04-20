using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Common.Interfaces;

public interface IAuditEventRepository
{
    Task AddAsync(AuditEvent auditEvent, CancellationToken cancellationToken);
    Task<IReadOnlyList<AuditEvent>> ListByTenantAsync(
        string tenantId,
        CancellationToken cancellationToken);
}
