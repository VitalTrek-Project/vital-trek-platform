using NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Commands;

namespace NexumDevs.VitalTrek.Platform.Navigation.Domain.Model.Aggregates;

public partial class Progress
{
    public Progress()
    {
        CompletedCheckpoints = 0;
        TotalCheckpoints = 0;
        Percentage = 0.0;
    }

    public Progress(CreateProgressCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        CompletedCheckpoints = command.CompletedCheckpoints;
        TotalCheckpoints = command.TotalCheckpoints;
        Percentage = command.Percentage;
    }
    
    public int Id { get; }
    
    public int CompletedCheckpoints { get; private set; }
    public int TotalCheckpoints { get; private set; }
    public double Percentage { get; private set; }
}
