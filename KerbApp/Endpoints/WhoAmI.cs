#pragma warning disable CA1416

using System.Security.Principal;
using FastEndpoints;

namespace KerbApp.Endpoints;

internal sealed class WhoAmI : EndpointWithoutRequest<WhoAmIRecord>
{
    public override void Configure()
    {
        Get(WhoAmIRequest.Route);
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        var user = (WindowsIdentity)HttpContext.User.Identity!;

        WindowsIdentity.RunImpersonated(user.AccessToken, () =>
        {
            var impersonated = WindowsIdentity.GetCurrent();

            Response = new WhoAmIRecord(
                impersonated.Name,
                impersonated.ImpersonationLevel.ToString());
        });
        
        return Task.CompletedTask;
    }
}

#pragma warning restore CA1416