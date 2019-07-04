	# Blazor With ServiceStack Demonstrations Demo13 ReadMe (at the Demo13 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo13, *Blazor GUI served by process having a Windows Service Lifetime

## Introduction
The Blazor GUI project ...
The server project focuses on the details of installing the server project as a Windows service, and partially automating that process.
The Common DTOs ..

## Publishing Steps
Demo13 adds a new section to the ReadMe for documenting what changes get made in the publishing process.
This demo focuses on automating the publishing process, and provides a PowerShell script `PublishingAutomation.ps1`
You will need to install a `.Net Core Global Tool` called `installUtil`. From a VS 2019 command prompt, run `dotnet tool install -g --framework netcoreapp3.0 --version 1.2.0 InstallUtil`. Of course check that this is still the latest version and adjust the instructions accordingly, if a later version exists.

## Blazor GUI
This demo focuses on getting the new `PublishedService.pubxml` file setup properly.
Details in [Demo13 Blazor GUI](GUI/ReadMe.html)

## Server
This demo focuses on automating the process of moving the code from a pure development location to a location akin to what the final installation location for the service will look like.
creating the basic windows service additions, then hosting the ServiceStack middleware in a Kestrel-only webHost inside a GenericHost under Net Core V3.0 and running it as a Service
Introduction of a PowerShell script to partially automate the process of distribution and installation
Install WiX Toolset V3.11.1 https://github.com/wixtoolset/wix3/releases/tag/wix3111rtm .exe file and run as administrator
WiX Toolset build tools RC for VS 2019 extension called Votive2019.vsix; install

Details in [Demo13 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

	
	: [Demo13 Overview](Documentation/Overview.html)
