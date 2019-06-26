# Blazor With ServiceStack Demonstrations Demo11 ReadMe (at the Demo11 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo11, *Blazor GUI served by process having a Windows Service Lifetime

## Introduction
The Blazor GUI project ...
The server project focuses on the details of installing the server project as a Windows service, and partially automating that process.
The Common DTOs ..

## Publishing Steps
Demo11 adds a new section to the ReadMe for documenting what changes get made in the publishing process.
This demo focuses on automating the publishing process, and provides a PowerShell script `PublishingAutomation.ps1`
You will need to install a `.Net Core Global Tool` called `installUtil`. From a VS 2019 command prompt, run `dotnet tool install -g --framework netcoreapp3.0 --version 1.2.0 InstallUtil`. Of course check that this is still the latest version and adjust the instructions accordingly, if a later version exists.

## Blazor GUI
This demo focuses on getting the new `PublishedService.pubxml` file setup properly.
Details in [Demo11 Blazor GUI](GUI/ReadMe.html)

## Server
ILWeaving

Details in [Demo11 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

	
	: [Demo10 Overview](Documentation/Overview.html)
