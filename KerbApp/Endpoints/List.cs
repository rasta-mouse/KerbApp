#pragma warning disable CA1416

using System.Security.Principal;
using FastEndpoints;

namespace KerbApp.Endpoints;

internal sealed class List : EndpointWithoutRequest
{
    private readonly IConfiguration _config;

    public List(IConfiguration config)
    {
        _config = config;
    }

    public override void Configure()
    {
        Get(ListFileRequest.Route);
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        var user = (WindowsIdentity)HttpContext.User.Identity!;

        WindowsIdentity.RunImpersonated(user.AccessToken, () =>
        {
            Response = Directory.GetFiles(_config["SharePath"]!);
        });
        
        return Task.CompletedTask;
    }
}

#pragma warning restore CA1416