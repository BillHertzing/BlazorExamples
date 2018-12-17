
# Attribution of ideas
The maintainers and contributors to this repository feel it is important to credit the individuals and organizations who have taken their time to publish ideas and guides. This document provides a place to mention ALL of teh works that have influenced the design and implementation of the repository's content.


## Documentation
### Creation of a repository or project's ReadMe file
  * https://github.com/noffle/art-of-readme
  * https://github.com/jehna/readme-best-practices
  * https://blog.algorithmia.com/github-readme-analyzer/
### Creation of a repository's documentation site
  * https://visualstudiomagazine.com/articles/2017/02/21/vs-dotnet-code-documentation-tools-roundup.aspx
  * https://dotnet.github.io/docfx/
### DocFx
<tbd> this should someday be created by a docfx merge plugin, to merge Attribution.md files found in each project, up to a master Attribution.html
<fyi, all the attribution lines are currently done manually (cut'n'paste)>
<multitude of docfx.json files from other early adopter's sites>
  * https://github.com/docascode/docfx-seed/blob/master/docfx.json
  * https://github.com/wekempf/testify/blob/develop/docs/docfx.json
  * https://github.com/googleapis/google-cloud-dotnet/blob/master/docs/root/docfx.json
  * https://github.com/SixLabors/docs/blob/master/docfx.json
  * https://dzone.com/articles/generating-documentation-with-docfx-as-part-of-a-v
### Adding triple-Slash \(///) comments to code
  * https://submain.com/products/ghostdoc.aspx * Commercial editions as well as free community version


## Visual Studio
###  Doing Builds 
#### Build Logging
  * Project System Tools (Structured MSBUILD log viewer) https://marketplace.visualstudio.com/items?itemName=VisualStudioProductTeam.ProjectSystemTools and http://msbuildlog.com/
#### New Project System for MSBuild
  * https://github.com/dotnet/project-system/blob/master/docs/opening-with-new-project-system.md"

#### Builds in Docker
  * https://natemcmaster.com/blog/2018/05/12/dotnet-watch-2.1/ Indicates need to add to .csproj. Then implies obj and bin(!) need subdirs of /local and /container

### Packing into NuGet packages
#### Packing Libraries
##### Library Versioning
These libraries use the following attributes for versioning
    * AssemblyInfo - <tbd> insert link for Semantic versioning
    * AssemblyFileInfo - <tbd> insert link for date since ? and secs/midnight / 2
    * AssemblyInformational - <tbd> insert links that show how modern NuGet resolves the third part in alphabetical order, so alpha resolve before beta, qa, rc, rtm, etc. tbd - link to the BuildTooling documentation in tyhis repository that provides the targets and tasks that update the version during a build if necessary.
##### Library Dependencies
##### Development Versioning and LocalFeed

#### Packing the BuildTools Assemblies
##### BuildTool Versioning
##### BuildTool Dependencies
##### Installing Executables in the user's .nuget
##### Installing PowerShell Scripts in the user's .nuget
##### Installing Documentation for the BuildTools and configuring Visual Studio to recognize it
##### Development Versioning and LocalFeed

#### Packing the Blazor-SserviceStack Examples Assemblies
##### App Versioning
##### App Dependencies
##### Chocolatey Installation script
##### Chocolatey Installation configuration
##### Development Versioning and LocalFeed

https://natemcmaster.com/blog/2017/11/11/build-tools-in-nuget/

# Obsolete
The following links are to projects or articles no longer included in the solution. However, they were considered relevant at one time, and certainly will contain information worth reviewing.

MSBump: Used for automatic build number bumping and Nuget Package versioning https://github.com/BalassaMarton/MSBump
