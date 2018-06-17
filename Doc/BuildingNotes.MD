<H1> Instructions for building this app 

# Versioning
- TBD

# Directory.Build.Props
- Do not put <TargetFramework> or <TargetFrameworks> in here

# AceGUI
- The .csproj should specify <TargetFrameworks>netstandard2.0</TargetFrameworks>. Note the use of the plural form; this ensures a subdirectory will be created under Debug and release for netstandard2.0

# Dependencies
The AceGUI project (client) should depend on ServiceStack.Interfaces assembly in order to share DTO projects/assemblies/classes with the AceAgent (server) executables. But it may not depend on ServiceStack assembly, as that brings in too many other subdependencies, which will break the ILLinker step in building if it is referenced.

## Publish Profiles
- There is a DebugFolderProfile and a ReleasePublishProfile. Both publish to the FileSystem. The Debug publishs to a subdir relative to the ACEGui project. The Release publishes to a Release subdirectory relative to the root of the solution.

## Nuget Packaging
- TBD

# AceService
Located just below the solution, this folder holds the projects/assemblies that make up the Windows-specific version of the AceAgent
## AceService
- The .csproj should specify  <TargetFramework>net47</TargetFramework>. Note the use of the singular form; this ensures that there is no subdirectory created below Debug.

## AceService.GUIServices.Models
- The .csproj should specify <TargetFrameworks>netstandard2.0;net47</TargetFrameworks>. This will ensure that assemblies are built for both target frameworks. One will be linked to the AceGUI, the other will be linked to the AceService.
- The project (server) should depend on ServiceStack.Interfaces assembly in order to share DTO projects/assemblies/classes with the AceGUI (client) Blazor app.

## AceService.GUIServices.Interfaces
- The .csproj should specify <TargetFrameworks>net47</TargetFrameworks>. Note the use of the plural form; this ensures a subdirectory will be created under Debug and Release  for netstandard2.0
- The project (server) should depend on ServiceStack.Interfaces assembly in order to share DTO projects/assemblies/classes with the AceGUI (client) Blazor app.

# NuGet Packages Used
## Blazor

## AceAgent
### AceService
Newtonsoft
TimePerdiolibrary
### UnitTesting
Xunit
MOQ
FluentAssertions