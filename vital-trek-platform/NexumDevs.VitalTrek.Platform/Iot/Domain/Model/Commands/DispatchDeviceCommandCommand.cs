using System;

namespace NexumDevs.VitalTrek.Platform.Iot.Domain.Model.Commands;

public record DispatchDeviceCommandCommand(int DeviceId, string CommandType, DateTime IssuedAt);