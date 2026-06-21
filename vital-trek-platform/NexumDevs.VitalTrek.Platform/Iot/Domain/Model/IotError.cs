namespace NexumDevs.VitalTrek.Platform.Iot.Domain.Model;

public enum IotError
{
    None,
    DeviceNotFound,
    InvalidDeviceType,
    InvalidSensorType,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}