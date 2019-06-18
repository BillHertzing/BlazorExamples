# Blazor With ServiceStack Demonstrations Demo08 ReadMe (at the common Demo08 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo08, *Adding detailed environment-specific configuration settings*

## Introduction
The Blazor GUI project ...
The server project 
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo08 Blazor GUI](GUI/ReadMe.html)

## Server
The static `IHostBuilder` method `CreateSpecificGenericHostBuilder()` adds AppConfiguration based on a chain of Configuration providers.
The AppConfiguration includes a base appsettings file and environment-specific appsettings files.

Details in [Demo08 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

