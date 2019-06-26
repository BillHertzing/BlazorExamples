# Blazor With ServiceStack Demonstrations Demo10 ReadMe (at the common Demo10 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo10, *Adding Event Tracing for Windows (ETW)*.

## Introduction
The Blazor GUI project ...
The server project 
The Common DTOs ..

## Blazor GUI
This demo focuses on 
Details in [Demo10 Blazor GUI](GUI/ReadMe.html)

## Server
This demo focuses on Tracing and Profiling
A managed ETW provider is added.
The ETW Provider class has two methods, that create ETW events for method entry and method exit
The Serilog logging messages for method entry and exit throughout the Server classes are replaced with the ETW methods 
A number of external tools useful for collecting, viewing, and analyzing ETW events are discussed in the documentation, along with links to further information
  1. PerfView, and how to use it to collect ETW events from the demo, and view the ETW logs
  1. WPA, Windows Performance Analyzer
 
 Note that current implementation is a bit hackey, please see https://github.com/dotnet/csharplang/issues/87 for a better solution, which will need to wait for implementation of CallerTypeName added to Caller Info attribute


Details in [Demo10 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`
	
	: [Demo10 Overview](Documentation/Overview.html)
