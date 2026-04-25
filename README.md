# Learning .NET

A personal repository for learning the .NET ecosystem (C#, tooling, libraries, and best practices) through small experiments and notes.

## Goals

- Learn modern C# and .NET (SDK-style projects, CLI, NuGet)
- Practice building small apps and libraries
- Capture notes and links while learning

## Getting started

### Prerequisites

- Install the **.NET SDK**: https://dotnet.microsoft.com/download
- (Optional) An editor like **Visual Studio**, **Visual Studio Code**, or **Rider**

### Verify your install

```bash
dotnet --info
```

### Run a project (example)

From the project directory:

```bash
dotnet restore
dotnet build
dotnet run
```

## Repository structure

- `src/` – Source code (apps/libraries)
- `tests/` – Unit/integration tests
- `notes/` – Learning notes and references

> If these folders don’t exist yet, they’ll be added as the repo grows.

## Useful commands

```bash
# create a new console app
dotnet new console -n HelloWorld

# create a solution and add the project
dotnet new sln -n LearningDotNet
dotnet sln add HelloWorld/HelloWorld.csproj

# run tests
dotnet test

# list installed SDKs
dotnet --list-sdks
```

## Topics to explore

- C# language features (records, pattern matching, LINQ)
- ASP.NET Core (minimal APIs, MVC)
- Dependency Injection and configuration
- EF Core and data access
- Logging and diagnostics
- Testing (xUnit/NUnit, mocking)
- Packaging and publishing (NuGet, `dotnet publish`)

## Contributing

This is primarily a learning repo, but suggestions and improvements are welcome. Open an issue or a PR.

## License

Add a license when you’re ready (e.g., MIT).
