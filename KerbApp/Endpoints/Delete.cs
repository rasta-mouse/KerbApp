#pragma warning disable CA1416

using System.Security.Principal;
using FastEndpoints;

namespace KerbApp.Endpoints;

internal sealed class Delete : Endpoint<DeleteFileRequest>
{
    private readonly IConfiguration _config;

    public Delete(IConfiguration config)
    {
        _config = config;
    }

    public override void Configure()
    {
        Delete(DeleteFileRequest.Route);
    }

    public override async Task HandleAsync(DeleteFileRequest req, CancellationToken ct)
    {
        var user = (WindowsIdentity)HttpContext.User.Identity!;

        await WindowsIdentity.RunImpersonatedAsync(user.AccessToken, async () =>
        {
            var path = Path.Combine(_config["SharePath"]!, req.Filename);

            if (!File.Exists(path))
            {
                await SendNotFoundAsync(ct);
                return;
            }

            File.Delete(path);
            await SendNoContentAsync(ct);
        });
    }
}

#pragma warning restore CA1416