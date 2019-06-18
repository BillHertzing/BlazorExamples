# Blazor With ServiceStack Demonstrations Demo07 ReadMe (at the Demo07 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo07, *Blazor GUI served by ServiceStack Middleware hosted by Kestrel-Only Web Host Hosted within a GenericHost V3.0.

## Introduction
The Blazor GUI project ...
The server project 
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo07 Blazor GUI](GUI/ReadMe.html)

## Server
The concept of Configuration (ConfigurationBuilder, Configuration Providers, and a ConfigurationRoot) is introduced 
An initial hostConfigurationRoot is created from InMemory, File, and EnvironmentVariable providers.
A new compilation unit is introduced `DefaultConfiguration.cs`, which contains the minimal configKeys needed to run the program in production (URLs)
LaunchSettings.json grows to include environment variables for the WebHostToBuild, Environment and URLs 
WebHostToBuild and Environment strings from the ConfigurationRoot are validated. If not present, defaults are provided and added to the ConfigurationRoot.
The static `IHostBuilder` method `CreateGenericHostBuilder()` grows an additional parameter of type `ConfigurationRoot`, and the `hostConfigurationRoot` is passed to the method
The Environment value retrieved from the hostConfigurationRoot is used to conditionally apply the Development-only configuration options.
The Development-only configuration options are moved into the static `CreateGenericHostBuilder()` method.

Details in [Demo07 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

	
	: [Demo07 Overview](Documentation/Overview.html)
	
