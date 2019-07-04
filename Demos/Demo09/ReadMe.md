# Blazor With ServiceStack Demonstrations Demo09 ReadMe (at the common Demo09 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo09, *Adding Serilog as a Microsoft.Extensions.Logging logger*.

## Introduction
The Blazor GUI project ...
The server project demonstrates how to add / reference SeriLog as a MEL logger, configure logging, and view logging in a stream database.
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo09 Blazor GUI](GUI/ReadMe.html)

## Server
This demo focuses on Logging
An external tool `Seq` is added to provide centralized log collection and viewing. Log messages are ingested as HTTP messages to `localhost` on the default port 5341
The `ServiceStack.Logging` dependency and using is removed from all compilation units (source .cs files)
The `Microsoft.Extensions.Logging` NuGet Package dependency is added to the project
Serilog dependencies (NuGet Packages) are added to the project: (`Serilog, Serilog.AspNetCore, Serilog.Enrichers.Thread,S erilog.Exceptions, Serilog.Settings.Configuration, Serilog.Sinks.Console, Serilog.Sinks.Debug, Serilog.Sinks.File, Serilog.Sinks.Seq, SerilogAnalyzer`)
The `genericHostSettings.json` and `genericHostSettings.development.json` files grow an extensive logging sections for `Microsoft.Extensions.Logging` and for Serilog
`genericHostSettings.json` (production) configures Serilog with the `Seq` writer. The Serilog `LogContext` is enriched with the current threadID, and an additional property, `Application` having the value 'Demo09"
`genericHostSettings.json` (Development) configures Serilog with the `Seq` writer, the `Console` writer, and the `DebugOutpu` writer. 
The Serilog static Log object is initialized with the Serilog configuration read from the ConfigurationRoot
The static method CreateGenericHost adds `Serilog.AspNetCore` logging to the genericHost's `ConfigureWebHostDefaults` builder extension and to the `.ConfigureLogging` builder extension via `.UseSerilog()`
The NLog.config and NLog.xsd files are removed. 
The Serilog Analyzer, installed via NuGet, will provide IntelliSense for the Serilog Log methods
The Log statements that used String expansion are replaced with SeriLog structured logging messages. The Analyzer provides a suggested replacement, so this step is just a "replace all occurrences" in the project

Details in [Demo09 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`
	
	: [Demo09 Overview](Documentation/Overview.html)
