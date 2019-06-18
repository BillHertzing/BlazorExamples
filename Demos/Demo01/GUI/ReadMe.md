# Blazor With ServiceStack Demonstrations Demo01 ReadMe for the GUI (at the Demo01/GUI Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

PreRelease Warning: Client-Side Blazor (CSB) is an experimental offering from Microsoft. which may be discontinued at any time
these demos use Clinet-Side Blazor. Current demos are built using .Net Core 3.0 Preview 5


## Introduction
The intent of the Blazor GUI project is to provide a GUI that interfaces with the Server project in each demo.
This document focuses on the details of the GUI in each demo

## Blazor GUI features
The Blazor GUI consists of a Home page.
The text in the Home page is all hardcoded.
There is C# code at all.

## Blazor GUI Build
As of .Net Core 3.0 Preview 5, the build tooling for CSB forces a Release build, regardless of the build configuration specified in Visual Studio or dotnet CLI

## Blazor GUI Publish 
The GUI project has a Publish profile for a Release build. This will not change for the first nine demos.
The target location for the Release Build is $OutputDir/Release/netstandard2.0/Publish.  This will not change for the first nine demos.
The Publish location has all the content needed for the application, and includes a web.config file.  This will not change for the first nine demos.
Below the Publish location are the subdirectories GUI/dist, which is the WWWRoot for the CSB GUI. This will not change for the first nine demos.

## Serving the GUI from any web server capable of serving static files