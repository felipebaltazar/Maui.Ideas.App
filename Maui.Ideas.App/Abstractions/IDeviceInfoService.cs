
namespace Maui.Ideas.App.Abstractions;

public interface IDeviceInfoService
{
    string Manufacturer { get; }
    DevicePlatform Platform { get; }
}
