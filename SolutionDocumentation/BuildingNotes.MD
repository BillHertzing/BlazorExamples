# Notes on Building the solution
If you are viewing this BuildingNotes.md in GitHub, [here is this same document on the documentation site](GettingStarted.html)

## Prerequisites
Details for the [Prerequisites](GettingStarted.html#Prerequisites)
Details for thhe [optional 3rd-party tools]()

## General Background Build Information
In the interest of "Don't Repeat Yourself (DRY)" principles, you will find general documentation on the build tools, versioning methodology, Documentation creation, etc, in the ATAP.Utilities repository.
    1.[Configuring and using the Visual Studio IDE and Visual Studio Team Explorer](https://billhertzing.github.io/ATAP.Utilities/SolutionDocumentation/Building.html). 
	2.[GitFlow branch and release strategy]() used by this repository.
	3.[Versioning and Packaging]() used by this repository.
	1.[Building Documentation]() general instructions for using DocFx to build documentation for a repository

## Versioning
This repository follows the general guidelines above. 

## Organization
Refer to the [Organization documentation](Organization.html) to pictures of how the solution and filesystem layouts are organized.

## Build Configuration information at the Solution level
The Solution Item solution folder level contains build configuration information that applies to all projects in the solution, as follows:

### Directory.Build.Props
- Tells all projects to use the latest C# language features, and that VersionInformation is explicitly defined in each project. Defines common compilation symbols and flags.
- TargetFrameworks

## Build Configuration information at the Demoxx level
Individual under each Demoxx folder are three projects. To build a working Demo, all three projects must be built, and the GUI project must also be published. Building notes for each project are below

### Build the ConsoleApp.exe
- The .csproj should specify \<TargetFramework>net471\</TargetFramework>.

### Build the CommonDTO library
- The .csproj should specify \<TargetFrameworks>netstandard2.0;net471\</TargetFrameworks>. Note the plural form of the Property. This will ensure that assemblies are built for both target frameworks.

## Build the GUI
- The .csproj should specify \<TargetFramework>netnetstandard2.0\</TargetFramework>.

## Publish the GUI

### Publish Profiles
- There is a DebugFolderProfile and a ReleaseFolderProfile. Both publish to the filesystem. The DebugFolderProfile publishes to a subfolder relative to the GUI project's folder. The ReleaseFolderProfile publishes to a Release subfolder relative to the root of the solution.

## NuGet Packaging
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
	
