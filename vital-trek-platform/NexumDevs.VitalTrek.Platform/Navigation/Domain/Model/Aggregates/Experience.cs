using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;
using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.ValueObjects;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;

public partial class Experience
{
    public Experience()
    {
        ExpeditionID = null!;
        TouristID = null!;
        Note = null!;
        MediaUrl = null!;
        CreatedAt = null!;
    }

    public Experience(CreateExperienceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ExpeditionID = command.ExpeditionID;
        TouristID = command.TouristID;
        Note = command.Note;
        MediaUrl = command.MediaUrl;
        CreatedAt = command.CreatedAt;
    }
    
    public int Id { get; }
    
    public ExpeditionId  ExpeditionID { get; private set; }
    public TouristId TouristID { get; private set; }
    public NoteItem Note { get; private set; }
    public string MediaUrl { get; private set; }
    public string CreatedAt { get; private set; }
}
