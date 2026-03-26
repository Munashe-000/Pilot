using PilotFlow.Application.Common.Interfaces;

namespace PilotFlow.Infrastructure.Services;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
