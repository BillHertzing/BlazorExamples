# Blazor With ServiceStack Demonstrations Demo03 ReadMe (at the Demo03 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo03, *Blazor GUI served by ServiceStack Middleware hosted in a Kestrel-Only WebHost*.

## Introduction
The Blazor GUI project introduces browser-local storage, and uses it to provide persistence for a page-local property.
The server project targets Net Core V2.2. A KestrelAlone WebHost is introduced.

## Blazor GUI
The GUI references the third-party Blazor library Blazored.LocalStorage.
The synchronous interface to Blazored.Localstorage is used to persist the page-local `AnIntegerProperty` via that property's getter and setter.
Details in [Demo03 Blazor GUI](GUI/ReadMe.html)

## Server
The server moves the SSApp middleware into a KestreAlone WebHost running under .Net Core V2.2.
A static method for the creation of an IWebHostBuilder configured to create a KestreAlone WebHost is introduced.
Within the static method,  a new WebHost is configured with no defaults by new()ing an instance of the WebHostBuilder class then applying the extension method .AddKestrel().
launchSettings.json refers to a KestrelOnly 

Details in [Demo03 Server](Server/ReadMe.html)

