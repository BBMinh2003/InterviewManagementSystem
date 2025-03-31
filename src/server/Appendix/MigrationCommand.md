# Migrations

## 1. Using the CLI

### Add a migration
```bash
dotnet ef migrations add AddBaseEntityModel --project IMS.Data --startup-project IMS.API --context IMSDbContext --output-dir Migrations
dotnet ef migrations add [MigrationName] --project IMS.API --startup-project IMS.API --context IMSDbContext --output-dir Migrations
dotnet ef migrations add UpdateUserNote --project IMS.Data --startup-project IMS.API --context IMSDbContext --output-dir Migrations
```

### Update the database
```bash
dotnet ef database update --project IMS.Data --startup-project IMS.API --context IMSDbContext
dotnet ef database update --project IMS.Data --startup-project IMS.API --context IMSDbContext
```

### Roll back a migration
```bash
dotnet ef database update [MigrationName] --project IMS.Data --startup-project IMS.API --context IMSDbContext
dotnet ef database update [MigrationName] --project IMS.Data --startup-project IMS.API --context IMSDbContext
```

### Drop the database
```bash
dotnet ef database drop --project IMS.API --startup-project IMS.API --context IMSDbContext
dotnet ef database drop --project IMS.Data --startup-project IMS.API --context IMSDbContext
```

### Remove a migration
```bash
dotnet ef migrations remove --project IMS.Data --startup-project IMS.API --context IMSDbContext
dotnet ef migrations remove --project IMS.Data --startup-project IMS.API --context IMSDbContext
```

## 2. Using the Package Manager Console
### Add a migration
```bash
Add-Migration [MigrationName] -Project IMS.Data -StartupProject IMS.API -Context IMSDbContext -OutputDir IMS.Data/Migrations
```

### Update the database
```bash
Update-Database -Project IMS.Data -StartupProject IMS.API -Context IMSDbContext
```

### Roll back a migration
```bash
Update-Database [MigrationName] -Project IMS.Data -StartupProject IMS.API -Context IMSDbContext
```

### Remove a migration
```bash
Remove-Migration -Project IMS.Data -StartupProject IMS.API -Context IMSDbContext
```

[]: # Path: README.md
