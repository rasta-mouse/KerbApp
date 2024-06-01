## Kerberos Delegation Test App

This is a simple ASP.NET Core web app to simulate various Kerberos delegation configurations.  Only supports kernel mode authentication on Windows.

### Configuration

Add a UNC path to a network share and an MS SQL connection string in `appsettings.json`.  Example:

```json
{
  "SharePath": "\\\\fs.contoso.com\\test$",
  "ConnectionStrings": {
    "Default": "Server=sql.contoso.com;Database=master;Integrated Security=true;trustServerCertificate=true;"
  }
}
```

### Build

```
dotnet publish --self-contained -r win-x64
```

### Run
```
KerbApp.exe --urls http://*:80
```