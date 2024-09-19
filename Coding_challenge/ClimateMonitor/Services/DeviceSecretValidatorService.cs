namespace ClimateMonitor.Services;

public sealed class DeviceSecretValidatorService : IDeviceSecretValidatorService
{
    private static readonly HashSet<string> ValidSecrets = new HashSet<string>
    {
        "secret-ABC-123-XYZ-001",
        "secret-ABC-123-XYZ-002",
        "secret-ABC-123-XYZ-003"
    };

    public bool ValidateDeviceSecret(string deviceSecret)
        => ValidSecrets.Contains(deviceSecret);

}

public interface IDeviceSecretValidatorService
{
    bool ValidateDeviceSecret(string deviceSecret);
}