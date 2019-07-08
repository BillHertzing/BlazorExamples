# Blazor With ServiceStack Demonstrations Demo04 ReadMe (at the Demo04 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo04, *Blazor GUI served by ServiceStack Middleware hosted by selectable WebHost (.Net Core V2.2)*.

## Introduction
In the GUI project, the site.sccs file introduces variables
The GUI project introduces State to provide state-aware browser-local storage and to provide state transitions.
The server project targets .Net Core 2.2. Launchsettings.json are introduced to allow the developer to select either of the two static WebHostBuilders when starting a debugging session.

## Blazor GUI
The site.sccs uses variables for From and To colors for the <body> background.
The GUI 
Details in [Demo04 Blazor GUI](GUI/ReadMe.html)

## Server
This demo focuses on adding the ability to allow the developer to select either of the two web server hosts, KestrelAlone or IISIntegratedInProcess, to be used when starting a debugging session for the project's executable. 
The server remains running under .Net Core V2.2
Environment Variables make their first appearance.
LaunchSettings.json is used to add one specific Environment Variable to the environment before a debugging session begins.
The concept of allowing the developer to select the WebHostBuilder at the start of a debugging session via the LaunchSettings is introduced.
Examples of selecting the WebHost for debugging sessions started from either Visual Studio or the CLI are discussed.

ToDo: Move following to details?
New in this Demonstration are 
1. a second entry in the launchSettings.json Profiles list
1. environment variables introduced into the individual entries of the launchSettings.json Profiles list
1. How to read environment variables before the web host is created so that the program can decide what kind of a web host to create


Details in [Demo04 Server](Server/ReadMe.html)







