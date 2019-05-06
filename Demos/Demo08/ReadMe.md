# Blazor With ServiceStack Demonstrations Demo08 ReadMe (at the common Demo08 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo08, *Blazor GUI served by ServiceStack Net Core in  Kestrel-only (Core3.0 P5)*.

## Introduction

This documents the types, pages, and services used by the this demonstration program

## CommonDTOs

The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

## Blazor GUI



## server

    This demo program builds off the previous demos. The Blazor GUI project and the CommonDTOs project remain unchanged from the previous demo. This demo focuses on hosting the ServiceStack middleware in a Kestrel-only webHost under Net Core V3.0 P5. .
	
	: [Demo08 Overview](Documentation/Overview.html)
