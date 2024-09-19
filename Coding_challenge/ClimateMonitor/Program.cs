using ClimateMonitor.Services;
using ClimateMonitor.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOptions<FirmwareValidationSetting>()
    .BindConfiguration(nameof(FirmwareValidationSetting))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddTransient<IAlertService, AlertService>();
builder.Services.AddTransient<IDeviceSecretValidatorService, DeviceSecretValidatorService>();
builder.Services.AddTransient<IFirmwareValidationService, FirmwareValidationService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
