using Maui.Ideas.App.Abstractions;

namespace Maui.Ideas.App.Infrastructure.Services;

public sealed class DeviceInfoService : IDeviceInfoService
{
    public string Manufacturer => DeviceInfo.Manufacturer;

    public DevicePlatform Platform => DeviceInfo.Platform;
}
