#pragma warning disable CA1416

using System.Security.Principal;
using FastEndpoints;

namespace KerbApp.Endpoints;

internal sealed class Get : Endpoint<GetFileRequest>
{
    private readonly IConfiguration _config;

    public Get(IConfiguration config)
    {
        _config = config;
    }

    public override void Configure()
    {
        Get(GetFileRequest.Route);
    }

    public override async Task HandleAsync(GetFileRequest req, CancellationToken ct)
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

            await using var fs = File.OpenRead(path);
            using var sr = new StreamReader(fs);
            var content = await sr.ReadToEndAsync(ct);

            await SendOkAsync(content, ct);
        });
    }
}

#pragma warning restore CA1416