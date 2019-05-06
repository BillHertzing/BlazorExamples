# Blazor With ServiceStack Demonstrations Demo07 ReadMe (at the common Demo07 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo07, *Blazor GUI served by ServiceStack Net Core hosted Kestrel-only.

## Introduction

This documents the types, pages, and services used by the this demonstration program

## CommonDTOs

The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

## Blazor GUI



## server

    This demo program builds off the previous demos. The Blazor GUI project and the CommonDTOs project remain unchanged from the previous demo. This demo focuses on hosting the ServiceStack middleware in a Kestrel-only webHost under Net Core V3.0. .
	
	: [Demo07 Overview](Documentation/Overview.html)
