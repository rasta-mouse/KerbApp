namespace KerbApp.Endpoints;

public sealed class CreateFileRequest
{
    public const string Route = "/files";

    public string Filename { get; set; }
    public string Content { get; set; }
}