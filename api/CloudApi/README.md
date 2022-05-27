# Uropatruljen - Cloud Api



* MSSQL connection string from installer: Server=localhost;Database=master;Trusted_Connection=True;

## NuGet Pakker installeret:
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Design
* System.Data.SqlClient
* Microsoft.AspNetCore.Authentication.JwtBearer

## Install .NET Entity Framework tools:
```bash
dotnet tool install --global dotnet-ef
```

## To generate migrations (diff)
```bash
dotnet ef migrations add InitialCreate --context UroContext
```

## To apply migrations
```bash
dotnet ef database update --context UroContext --connection "Server=localhost; Database=uro_db; User Id=sa; Password=12345; Trusted_Connection=True; TrustServerCertificate=True;"
```

## Lets Encrypt via certbot
```bash
sudo apt install certbot python3-certbot-apache
sudo certbot --apache -d uroapp.dk -d www.uroapp.dk
```