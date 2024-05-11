## Kerberos Delegation Test App

This is a simple ASP.NET Core web app to simulate various Kerberos delegation configurations.  Only supports kernel mode authentication on Windows.

### Build

```
dotnet publish --self-contained -r win-x64
```

### Run
```
KerbApp.exe --urls http://*:80
```