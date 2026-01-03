using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using AbpSaasPlatform.Application;
using AbpSaasPlatform.EntityFrameworkCore;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.PermissionManagement;

namespace AbpSaasPlatform.HttpApi.Host;

[DependsOn(
    typeof(AbpSaasPlatformApplicationModule),
    typeof(AbpSaasPlatformEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpOpenIddictAspNetCoreModule)
)]
public class AbpSaasPlatformHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        ConfigureAuthentication(context, configuration);
        ConfigureCors(context, configuration);
        ConfigureHealthChecks(context);

        Configure<AbpBackgroundWorkerOptions>(options =>
        {
            // Disable background workers (e.g., OpenIddict cleanup) to avoid startup failures in this deployment
            options.IsEnabled = false;
        });

        Configure<PermissionManagementOptions>(options =>
        {
            // Skip dynamic permission store to avoid static permission writes during startup
            options.IsDynamicPermissionStoreEnabled = false;
        });

        // Replace static permission saver with no-op to avoid tenant-less repository usage at startup
        context.Services.Replace(ServiceDescriptor.Transient<IStaticPermissionSaver, NullStaticPermissionSaver>());
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var corsOrigins = configuration["App:CorsOrigins"]?.Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(o => o.Trim().TrimEnd('/')).ToArray() ?? Array.Empty<string>();

        context.Services.AddCors(options =>
        {
            options.AddPolicy("Default", builder =>
            {
                builder.WithOrigins(corsOrigins)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddHealthChecks();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Localization settings not initialized; skip request localization to allow startup
        // app.UseAbpRequestLocalization();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("Default");
    }
}
