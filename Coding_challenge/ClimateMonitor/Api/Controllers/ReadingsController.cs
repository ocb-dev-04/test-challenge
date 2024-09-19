using Microsoft.AspNetCore.Mvc;
using ClimateMonitor.Services;
using ClimateMonitor.Services.Models;

namespace ClimateMonitor.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReadingsController : ControllerBase
{
    private readonly IDeviceSecretValidatorService _secretValidator;
    private readonly IAlertService _alertService;
    private readonly IFirmwareValidationService _firmwareService;

    public ReadingsController(
        IDeviceSecretValidatorService secretValidator,
        IAlertService alertService,
        IFirmwareValidationService firmwareService)
    {
        _secretValidator = secretValidator;
        _alertService = alertService;
        _firmwareService = firmwareService;
    }

    /// <summary>
    /// Evaluate a sensor readings from a device and return possible alerts.
    /// </summary>
    /// <remarks>
    /// The endpoint receives sensor readings (temperature, humidity) values
    /// as well as some extra metadata (firmwareVersion), evaluates the values
    /// and generate the possible alerts the values can raise.
    /// 
    /// There are old device out there, and if they get a firmwareVersion 
    /// format error they will request a firmware update to another service.
    /// </remarks>
    /// <param name="deviceSecret">A unique identifier on the device included in the header(x-device-shared-secret).</param>
    /// <param name="deviceReadingRequest">Sensor information and extra metadata from device.</param>
    [HttpPost("evaluate")]
    public ActionResult<IEnumerable<Alert>> EvaluateReading(
        [FromHeader(Name = "x-device-shared-secret")] string deviceSecret,
        [FromBody] DeviceReadingRequest deviceReadingRequest)
    {
        if (!_secretValidator.ValidateDeviceSecret(deviceSecret))
        {
            return Problem(
                detail: "Device secret is not within the valid range.",
                statusCode: StatusCodes.Status401Unauthorized);
        }

        if (!_firmwareService.CheckSemantic(deviceReadingRequest.FirmwareVersion))
        {
            ValidationProblemDetails? validationProblemDetails = new ValidationProblemDetails();
            validationProblemDetails.Errors.Add("FirmwareVersion", new[] { "The firmware value does not match semantic versioning format." });

            return BadRequest(validationProblemDetails);
        }

        return Ok(_alertService.GetAlerts(deviceReadingRequest));
    }
}
