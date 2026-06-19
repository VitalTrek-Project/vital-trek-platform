namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

public class MapReading
{
    public MapReading () {}

    public MapReading (int userId, int completedCheckpoints, int totalCheckpoints)
    {
        UserId = userId;
        CompletedCheckpoints = completedCheckpoints;
        TotalCheckpoints = totalCheckpoints;
    }
    
    public int UserId { get; set; }
    public int CompletedCheckpoints { get; set; }
    public int TotalCheckpoints { get; set; }
}
