namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

public class PanelReading
{
    public PanelReading() {}

    public PanelReading(int expeditionId, int totalCheckpoints, int completedCheckpoints)
    {
        ExpeditionId = expeditionId;
        TotalCheckpoints = totalCheckpoints;
        CompletedCheckpoints = completedCheckpoints;
    }
    
    public int  ExpeditionId { get; set; }
    public double TotalCheckpoints { get; set; }
    public double CompletedCheckpoints { get; set; }
}
