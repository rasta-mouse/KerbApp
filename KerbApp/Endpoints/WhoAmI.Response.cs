namespace KerbApp.Endpoints;

public sealed record WhoAmIRecord(
    string Username,
    string ImpersonationLevel);