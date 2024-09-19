using ClimateMonitor.Settings;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace ClimateMonitor.Services;

public sealed class FirmwareValidationService : IFirmwareValidationService
{
    private readonly string _regexExp;

    public FirmwareValidationService(IOptions<FirmwareValidationSetting> options)
    {
        _regexExp = options.Value.RegexPattern;
    }

    public bool CheckSemantic(string version)
    {
        Match result = new Regex(_regexExp).Match(version);

        return result.Success;
    }
}

public interface IFirmwareValidationService
{
    bool CheckSemantic(string version);
}