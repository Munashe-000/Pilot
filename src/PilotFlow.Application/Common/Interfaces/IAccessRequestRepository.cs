using PilotFlow.Domain.Entities;

namespace PilotFlow.Application.Common.Interfaces;

public interface IAccessRequestRepository
{
    Task AddAsync(AccessRequest request, CancellationToken cancellationToken);
    Task<AccessRequest?> GetByIdAsync(string tenantId, Guid id, CancellationToken cancellationToken);
}
