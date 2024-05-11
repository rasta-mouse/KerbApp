namespace KerbApp.Endpoints;

public sealed class DeleteFileRequest
{
    public const string Route = "/files/{Filename}";
    public string Filename { get; set; }
}