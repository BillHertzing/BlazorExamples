# Blazor With ServiceStack Demonstrations Demo08 ReadMe (at the common Demo08 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo08, *Adding detailed environment-specific configuration settings*

## Introduction
The Blazor GUI project ...
The server project adds the whole `Microsoft.Extensions.Configuration` thing, including the concept of `Environment` (Production, Development, Testing, etc.)
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo08 Blazor GUI](GUI/ReadMe.html)

## Server
The static `IHostBuilder` method `CreateSpecificGenericHostBuilder()` adds AppConfiguration based on a chain of Configuration providers.
The AppConfiguration includes 
  1. CommandLine Arguments
  1. Environment variables, filtered by a prefix
  1. How to branch the flow of execution based on `Environment`, and how to name the settings files.
  1. Settings file(s) in JSON format, Production, and additional settings based on Environment, for both the GenericHost and the WebHost
  1. compiled-in defaults for the `Environment`

Details in [Demo08 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

