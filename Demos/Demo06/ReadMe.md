# Blazor With ServiceStack Demonstrations Demo06 ReadMe (at the Demo06 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo06, *Blazor GUI served by ServiceStack Middleware hosted by Kestrel-Only Web Host Hosted within a Windows Service*.

## Introduction
The Blazor GUI project ...
The server project adds an enumeration for `SupportedWebHostBuilders` and validates/parses the corresponding environment variable, and merges the two static `IWebHostBuilders` into a single parameterized static builder method `CreateSpecificHostBuilder(SupportedWebHostBuilders webHostBuilderToBuild)`
The Common DTOs ..

## Blazor GUIses on 
Details in [Demo06 Blazor GUI](GUI/ReadMe.html)

## Server

This demo focuses on a single static builder for the GenericHost, which incorporates different webHostBuilder configurations based on a parameter to the method. The multiple static builders are removed.
A new enumeration `SupportedWebHostBuilders`is introduced, and the environment variable `BlazorDemos_WebHostBuilder`, which is a string, is validated, and only one of the enumerations selected to be the value of the `webHostBuilderToBuild` parameter to the `CreateSpecificGenericHostBuilder` static method
A new compilation unit is introduced `Enumerations.cs`. The class `Program` in `Program.cs` is made into a `partial` class, and the new Enumerations.cs also contains a declaration of `partial Program`

Details in [Demo06 Server](Server/ReadMe.html)

## CommonDTOs
This demo focuses on ...
The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

	
	: [Demo06 Overview](Documentation/Overview.html)

