# Notes on Building the solution

## Versioning
- Only in Github, no versioning done in any AssemblyInfo.cs file

# Directory.Build.Props
- Tells all projects to use the latest C# language features, and that VersionInformation is explicitly defined in each project. Defines common compilation symbols and flags.

## ConsoleApp
- The .csproj should specify \<TargetFramework>net471\</TargetFramework>.

## GUI
- The .csproj should specify \<TargetFramework>netnetstandard2.0\</TargetFramework>.

## CommonDTo
- The .csproj should specify \<TargetFrameworks>netstandard2.0;net471\</TargetFrameworks>. Note the plural form of the Property. This will ensure that assemblies are built for both target frameworks.

## Publish Profiles
- There is a DebugFolderProfile and a ReleaseFolderProfile. Both publish to the filesystem. The DebugFolderProfile publishes to a subfolder relative to the GUI project's folder. The ReleaseFolderProfile publishes to a Release subfolder relative to the root of the solution.

# NuGet Packaging
- N/A, none of the projects in this solution create NuGet packages

# NuGet Packages Used
    * Demo01 and all others
	    * GUI Project
			* Blazor (all current NuGet packages)
			* Blazor Logging Extensions
	    * ServiceStack Project
			* ServiceStack (At least version 5.4.1 or later)
			* NLog
    * Demo02 
	    * GUI Project
			* ServiceStack.Text
			
# Visual Studio Extensions to make coding and debugging easier
    * [Structured Build Logs]()
	* [Structured Build Log Viewer]()
	* [DocFx]()
	
# 3-rd party tools  to make coding and debugging easier
    * [Sentinal]() UDP Log sink
	
# Building under Visual Studio

# Building with MSBuild from the command line

# Building and hosting the documentation website with DocFx
	
