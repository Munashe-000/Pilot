using System.Collections.Concurrent;
using PilotFlow.Domain.Entities;

namespace PilotFlow.Infrastructure.Persistence.InMemory;

public sealed class InMemoryDataStore
{
    public ConcurrentDictionary<string, ConcurrentDictionary<Guid, AccessRequest>> AccessRequests { get; } = new();
    public ConcurrentDictionary<string, ConcurrentDictionary<Guid, TaskAssignment>> TaskAssignments { get; } = new();
    public ConcurrentDictionary<string, ConcurrentDictionary<Guid, AuditEvent>> AuditEvents { get; } = new();
}
