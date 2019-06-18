# Blazor With ServiceStack Demonstrations Demo04 ReadMe (at the Demo04 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo04, *Blazor GUI served by ServiceStack Middleware hosted by Kestrel-Only Web Host Hosted within a Windows Service*.

## Introduction
The Blazor GUI project ...
The server project targets .Net Core 2.2. Launchsettings.json are introduced to allow the developer to select either of the two static WebHostBuilders when starting a debugging session.
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo04 Blazor GUI](GUI/ReadMe.html)

## Server
This demo focuses on adding the ability to allow the developer to select either of the two web server hosts, KestrelAlone or IISIntegratedInProcess, to be used when starting a debugging session for the project's executable. 
The server remains running under .Net Core V2.2
Environment Variables make their first appearance.
LaunchSettings.json is used to add one specific Environment Variable to the environment before a debugging session begins.
The concept of allowing the developer to select the WebHostBuilder at the start of a debugging session via the LaunchSettings is introduced.
Examples of selecting the WebHost for debugging sessions started from either Visual Studio or the CLI are discussed.

### New in this demo
1. a second entry in the launchSettings.json Profiles list
1. one environment variable introduced into the individual entries of the launchSettings.json Profiles list
1. How to read environment variables before the web host is created so that the program can decide what kind of a web host to create
1. Selectively creating a genericWebHostBuilder by selecting one or the other of the two static IWebHostBuilder based on the environment variable value

Details in [Demo04 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

	
	: [Demo04 Overview](Documentation/Overview.html)

## Introduction

New in this Demonstration are 
1. a second entry in the launchSettings.json Profiles list
1. environment variables introduced into the individual entries of the launchSettings.json Profiles list
1. How to read environment variables before the web host is created so that the program can decide what kind of a web host to create

## CommonDTOs

## Blazor GUI





