using UpTulse.Application.Enums;
using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    public record MonitoringOperation(MonitoringTargetRequest Target, OperationType Type);
}