namespace KerbApp.Endpoints;

public sealed class GetFileRequest
{
    public const string Route = "/files/{Filename}";
    public string Filename { get; set; }
}