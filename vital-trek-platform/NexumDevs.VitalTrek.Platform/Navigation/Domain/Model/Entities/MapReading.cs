namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

public class MapReading
{
    public MapReading () {}

    public MapReading (int expeditionId, int completedCheckpoints, int totalCheckpoints)
    {
        ExpeditionId = expeditionId;
        CompletedCheckpoints = completedCheckpoints;
        TotalCheckpoints = totalCheckpoints;
    }
    
    public int ExpeditionId { get; set; }
    public int CompletedCheckpoints { get; set; }
    public int TotalCheckpoints { get; set; }

    public double RouteProgress (int completedCheckpoints, int totalCheckpoints)
    {
        if (totalCheckpoints == 0)
        {
            return 0.0;
        }
        
        double r1 = (double)completedCheckpoints / totalCheckpoints;
        double r = r1 * 100;
        return r;
    }
}
