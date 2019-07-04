# Blazor With ServiceStack Demonstrations Demo12 ReadMe (at the Demo12 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo12, *Blazor GUI served by ServiceStack Middleware hosted by Kestrel-Only Web Host hosted in a GenericHost having a Windows Service Lifetime

## Introduction
The Blazor GUI project adds the ability to publish to a specific folder within the _\_PublishedAgent\PublishedService_ subdirectory tree using the `PublishedService.pubxml` publishing option
The server project grows into a Windows Service
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo12 Blazor GUI](GUI/ReadMe.html)

## Server
This demo focuses on creating the basic windows service additions, then hosting the ServiceStack middleware in a Kestrel-only webHost inside a GenericHost under Net Core V3.0 and running it as a Service
The differences between a ConsoleApp and a Service are explored
A key runtime variable is introduced, IsConsoleApp, as are concepts from the Runtime, to determine if the program is running under Windows or Linux
switchMappings are added to the program's ConfigurationRoot to detect -C or -Console as a commandline switch
Instructions for using sc.exe for manually installing and uninstalling the genericHost as a Windows Service are documented 
Details in [Demo12 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

	
	: [Demo12 Overview](Documentation/Overview.html)
