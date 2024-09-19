using System.ComponentModel.DataAnnotations;

namespace ClimateMonitor.Settings;

public sealed class FirmwareValidationSetting
{
    [Required]
    public string RegexPattern { get; set; }
}
