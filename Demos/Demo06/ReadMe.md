# Blazor With ServiceStack Demonstrations Demo01 ReadMe (at the common Demo01 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo01, *Blazor GUI served by ServiceStack having two REST endpoints*.

## Introduction

This documents the types, pages, and services used by the first demonstration program

## CommonDTOs

The demonstration illustrates two service endpoints. 

| Route | Service Name | Verb | RequestDTO Type Name | ResponseDTO type Name|
|---|---|---|---|---||
|`/Initialization` | `BaseServices` | `Post` | `InitializationReqDTO` | `InitializationRspDTO`

## Blazor GUI



## ConsoleApp

    The first example program is the most basic. The Blazor GUI portion consists of two Razor pages, and code that makes two REST calls to ServiceStack, one REST call with no data payload sent or received, and one that sends a string and receives a string. The ServiceStack application portion consist of a Console program for .Net (full framework) which serves the static files for the Blazor application, and handles the two simple REST service endpoints. The CommonDTOs project defines the data payload sent and received between the ConsoleApp and the Blazor GUI.
	
	: [Demo01 Overview](Documentation/Overview.html)
