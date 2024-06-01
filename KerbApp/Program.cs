using System.Runtime.InteropServices;
using KerbApp.Components;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;

namespace KerbApp;

public class Program
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

        builder.Services.AddMudServices();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddDbContext<AppDbContext>(o =>
            o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        builder.Services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);
        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();

        builder.WebHost.UseHttpSys(o =>
        {
            // change scheme as desired
            o.Authentication.Schemes = AuthenticationSchemes.Negotiate;
            o.Authentication.AllowAnonymous = false;
        });

        builder.Host.UseSerilog((_, cfg) =>
            cfg.ReadFrom.Configuration(builder.Configuration));

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}