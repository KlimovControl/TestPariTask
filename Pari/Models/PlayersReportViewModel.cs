using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pari.Models;

public class PlayersReportViewModel
{
    public IEnumerable<PlayersReportModel> PlayersReportModels { get; set; }
    public bool? isBetHigher { get; set; }
    public SelectList PlayerStatuses { get; set; }
}