# Blazor With ServiceStack Demonstrations Demo01 ReadMe (at the common Demo01 Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo01, *Blazor GUI served by ServiceStack hosted in HTTPListener under Framework 4.7.1.

## Introduction
The Blazor GUI project targets Dot Net Standard 2.0. A basic Blazor Home page is created and delivered. 
The ConsoleApp project targets .NetFramework 4.7.1, and the HTTPListener WebHost.
The ServiceStack.Logging.Nlog logging provider is used.
The Common DTOs are not used in Demo01.

## Blazor GUI
This demo focuses on delivering the files needed to create/run a Blazor application in a browser.

Details in [Demo01 Blazor GUI](GUI/ReadMe.html)

## ConsoleApp

    The first example program is the most basic. The Blazor GUI portion consists of two Razor pages, and code that makes two REST calls to ServiceStack, one REST call with no data payload sent or received, and one that sends a string and receives a string. The ServiceStack application portion consist of a Console program for .Net (full framework) which serves the static files for the Blazor application, and handles the two simple REST service endpoints. The CommonDTOs project defines the data payload sent and received between the ConsoleApp and the Blazor GUI.
	
	: [Demo01 Overview](Documentation/Overview.html)
