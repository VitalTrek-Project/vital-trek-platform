namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Entities;

public class BinnacleReading
{
    public BinnacleReading() {}

    public BinnacleReading (int expeditionId, int touristId, string note, string mediaUrl, string createdAt)
    {
        ExpeditionId = expeditionId;
        TouristId = touristId;
        Note = note;
        MediaUrl = mediaUrl;
        CreatedAt = createdAt;
    }
    
    public int ExpeditionId { get; set; }
    public int TouristId { get; set; }
    public string Note { get; set; }
    public string MediaUrl { get; set; }
    public string CreatedAt { get; set; }
}
