namespace NexumDevs.VitalTrek.Platform.Dashboard.Domain.Model.ReadModels;

public record KpiValue(int Value, int PreviousValue)
{
    public double DeltaPercentage => PreviousValue == 0
        ? (Value > 0 ? 100d : 0d)
        : Math.Round((Value - PreviousValue) * 100d / PreviousValue, 1);
}
