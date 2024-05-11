#pragma warning disable CA1416

using System.Security.Principal;
using FastEndpoints;

namespace KerbApp.Endpoints;

internal sealed class Create : Endpoint<CreateFileRequest>
{
    private readonly IConfiguration _config;

    public Create(IConfiguration config)
    {
        _config = config;
    }

    public override void Configure()
    {
        Post(CreateFileRequest.Route);
    }

    public override async Task HandleAsync(CreateFileRequest req, CancellationToken ct)
    {
        var user = (WindowsIdentity)HttpContext.User.Identity!;

        await WindowsIdentity.RunImpersonatedAsync(user.AccessToken, async () =>
        {
            var path = Path.Combine(_config["SharePath"]!, req.Filename);

            await using var fs = File.CreateText(path);
            await fs.WriteAsync(req.Content);

            await SendCreatedAtAsync<Get>(new { req.Filename }, req.Content,
                cancellation: ct);
        });
    }
}

#pragma warning restore CA1416