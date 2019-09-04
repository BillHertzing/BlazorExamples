# Blazor With ServiceStack Demonstrations Demo10 ReadMe (at the common Demo10 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo10, *Adding Event Tracing for Windows (ETW)*.

## Introduction
The Blazor GUI project ...
The server project logs method boundries in the tracing window (not getters or setters yet) 
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo10 Blazor GUI](GUI/ReadMe.html)

## Server
This demo focuses on Tracing and Profiling
A managed ETW provider, named `DemoETWProvider`, derived from `EventSource` is added.
The `DemoETWProvider` class has one static Property, `Log`, which holds an instance of the `DemoETWProvider` class.
The `DemoETWProvider` has one method, `Information`, which writes to the ETW subsystem via `System.Diagnostics.Tracing`.
The Serilog logging messages for method entry and exit throughout the Server classes are replaced with calls to the `DemoETWProvider.Log.Information`
A number of external tools useful for collecting, viewing, and analyzing ETW events are discussed in the documentation, along with links to further information
  1. Visual Studio Diagnostics Event window
  1. PerfView, and how to use it to collect ETW events from the demo, and view the ETW logs
  1. ToDo: --WPA, Windows Performance Analyzer--
 
Details in [Demo10 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`
	
	: [Demo10 Overview](Documentation/Overview.html)
