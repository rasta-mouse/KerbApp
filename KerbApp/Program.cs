using System.Runtime.InteropServices;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Server.HttpSys;
using Serilog;

namespace KerbApp;

internal static class Program
{
    public static void Main(string[] args)
    {
        var logger = Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            logger.Fatal("This app will only run on Windows");
            return;
        }
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);
        builder.Services.AddAuthorization();
        builder.Services.AddFastEndpoints()
            .SwaggerDocument();

        builder.WebHost.UseHttpSys(opts =>
        {
            // change scheme as desired
            opts.Authentication.Schemes = AuthenticationSchemes.Negotiate;
            opts.Authentication.AllowAnonymous = false;
        });
        
        builder.Host.UseSerilog((_, cfg) =>
        {
            cfg.ReadFrom.Configuration(builder.Configuration);
        });
        
        var app = builder.Build();
        
        app.UseFastEndpoints()
            .UseSwaggerGen();

        app.Run();
    }
}