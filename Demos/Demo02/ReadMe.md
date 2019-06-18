# Blazor With ServiceStack Demonstrations Demo02 ReadMe (at the Demo02 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo02, *Blazor GUI served by ServiceStack Middleware hosted by Kestrel-Only Web Host Hosted within a Windows Service*.

## Introduction
The Blazor GUI project explorers 
The server project targets Net Core V2.1. An IntegratedIISInProcess WebHost is introduced.
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo02 Blazor GUI](GUI/ReadMe.html)

## Server
The Server moves the SSApp middleware into an IntegratedIISInProcess WebHost running under .Net Core V2.1
A static method for the creation of an IWebHostBuilder configured to create an IntegratedIISInProcess WebHost is introduced.
Within the static method, the extension method WebHost.CreateDefaultBuilder() is used to return an IWebHost pre-configured with the defaults necessary to run as an IntegratedIISInProcess WebHost
Details in [Demo02 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...

Details in [Demo02 Common DTOs](CommonDTOs/ReadMe.html)
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

	
	: [Demo02 Overview](Documentation/Overview.html)
