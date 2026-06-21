using System;
using NexumDevs.VitalTrek.Platform.Iot.Domain.Model.ValueObjects;
using NexumDevs.VitalTrek.Platform.Shared.Domain.Model;

namespace NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Aggregate;

public class IoTDevice : AuditableModel
{
    public IoTDevice()
    {
        Name = string.Empty;
    }

    public IoTDevice(string name, EDeviceType type, EDeviceStatus status, int? expeditionId, int? touristId)
    {
        Name = name;
        Type = type;
        Status = status;
        ExpeditionId = expeditionId;
        TouristId = touristId;
        LastSeen = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public EDeviceType Type { get; private set; }
    public EDeviceStatus Status { get; private set; }
    public DateTime? LastSeen { get; private set; }
    public string? LastCommand { get; private set; }

    // References by ID to other bounded contexts — no EF navigation properties
    public int? ExpeditionId { get; private set; }  // Navigation BC
    public int? TouristId { get; private set; }     // IAM BC (pending)

    public void DispatchCommand(string commandType, DateTime issuedAt)
    {
        LastCommand = commandType;
        LastSeen = issuedAt;
    }

    public void MarkOnline()
    {
        Status = EDeviceStatus.Online;
        LastSeen = DateTime.UtcNow;
    }

    public void MarkOffline()
    {
        Status = EDeviceStatus.Offline;
    }
}