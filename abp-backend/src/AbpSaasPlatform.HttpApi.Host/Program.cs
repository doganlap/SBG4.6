using Serilog;
using Serilog.Events;
using Volo.Abp;
using AbpSaasPlatform.EntityFrameworkCore;
using AbpSaasPlatform.HttpApi.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddAppSettingsSecretsJson()
    .UseSerilog((context, loggerConfig) =>
    {
        loggerConfig
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/app-.txt", rollingInterval: RollingInterval.Day);
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://admin.doganconsult.com",
                "https://tenant.doganconsult.com",
                "https://portal.doganconsult.com",
                "http://localhost:3000",
                "http://localhost:3001",
                "http://localhost:3002"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

await builder.AddApplicationAsync<AbpSaasPlatformHttpApiHostModule>();

var app = builder.Build();

await app.InitializeApplicationAsync();

app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");

Log.Information("Starting AbpSaasPlatform.HttpApi.Host");
await app.RunAsync();
