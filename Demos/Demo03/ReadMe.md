# Blazor With ServiceStack Demonstrations Demo03 ReadMe (at the Demo03 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo03, *Blazor GUI served by ServiceStack Middleware hosted by Kestrel-Only Web Host Hosted within a Windows Service*.

## Introduction
The Blazor GUI project ...
The server project targets Net Core V2.2. A KestrelAlone WebHost is introduced.
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo03 Blazor GUI](GUI/ReadMe.html)

## Server
The server moves the SSApp middleware into a KestreAlone WebHost running under .Net Core V2.2
A static method for the creation of an IWebHostBuilder configured to create a KestreAlone WebHost is introduced.
Within the static method,  a new WebHost is configured with no defaults by new()ing an instance of the WebHostBuilder class then applying the extension method .AddKestrel()

### New in this demo
1. a second entry in the launchSettings.json Profiles list

Details in [Demo03 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

	
	: [Demo03 Overview](Documentation/Overview.html)
	
	# Demo03 Blazor GUI using teh SS HttpClient-based JsonHttpClient
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)
    The third example program brings in the ServiceStack HttpClient-based JsonHttpClient as the communication library between the GUI and the ConsoleApp
	