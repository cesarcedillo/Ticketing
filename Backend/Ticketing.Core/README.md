# 📦 Using Internal NuGet Packages in Your .NET Solution

This guide explains how to create your own NuGet packages from shared projects (such as `core` libraries) and how to configure your solution to use these internal packages. This enables easy versioning, modularity, and future extraction of reusable code.

---

## 1. Packaging a Project as a NuGet Package

### 1.1. Prepare your `.csproj`

Make sure your project file (`.csproj`) includes the necessary metadata:

```xml
<PropertyGroup>
  <TargetFramework>net9.0</TargetFramework>
  <Version>1.0.0</Version> <!-- Update as needed -->
  <Authors>Your Name</Authors>
  <PackageId>MyCompany.Core.Domain.SeedWork</PackageId>
  <Description>Seed work base classes for domain-driven design.</Description>
</PropertyGroup>
```

* Set the `<Version>` to the desired package version.
* Set `<PackageId>` to a unique name for your package.

---

### 1.2. Build the NuGet Package

From your solution root (adjust paths as necessary):

```sh
dotnet pack src/core/Ticketing.Core.Domain.SeedWork/Ticketing.Core.Domain.SeedWork.csproj -o ./nuget-packages
```

* This will generate a `.nupkg` file in the `nuget-packages` directory.
* You can override the version inline:

  ```sh
  dotnet pack src/core/Ticketing.Core.Domain.SeedWork/Ticketing.Core.Domain.SeedWork.csproj -p:Version=1.1.0 -o ./nuget-packages
  ```

---

## 2. Using Your NuGet Package in Other Projects

### 2.1. Configure Your NuGet Feed

Add a `nuget.config` file to your solution root (or update an existing one) to include your local package directory:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="LocalPackages" value="./nuget-packages" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```

* Now your solution will look for packages in the `nuget-packages` folder first.

---

### 2.2. Reference the Package in a Project

In the `.csproj` of any microservice or consumer project, add a `<PackageReference>`:

```xml
<ItemGroup>
  <PackageReference Include="Ticketing.Core.Domain.SeedWork" Version="1.0.0" />
</ItemGroup>
```

* Replace the version and name according to your actual NuGet package.
* Remove any old `<ProjectReference>` to the same project.
