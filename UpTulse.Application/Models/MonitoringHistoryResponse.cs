using Facet;

using UpTulse.Core.Entities;

namespace UpTulse.Application.Models
{
    [Facet(typeof(MonitoringHistory), exclude: nameof(MonitoringHistory.CreatedBy))]
    public partial class MonitoringHistoryResponse
    {
    }
}