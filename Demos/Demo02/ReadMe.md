# Blazor With ServiceStack Demonstrations Demo02 ReadMe (at the Demo02 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo02, *Blazor GUI served by ServiceStack Middleware hosted in an Integrated IIS In-Process WebHost*.

## Introduction
The Blazor GUI project introduces page-local properties, a button, and the button's OnClick handler 
The server project targets Net Core V2.1. An IntegratedIISInProcess WebHost is introduced.

## Blazor GUI
The GUI implements a page-local property `AnIntegerProperty`.
The GUI implements a `<button>` element that references an `OnClick` handler.
The GUI implements a simple `OnClick` handler which increments the value of the `AnIntegerProperty`.
Details in [Demo02 Blazor GUI](GUI/ReadMe.html)

## Server
The Server moves the SSApp middleware into an IntegratedIISInProcess WebHost running under .Net Core V2.1.
A static method for the creation of an IWebHostBuilder configured to create an IntegratedIISInProcess WebHost is introduced.
Within the static method, the extension method WebHost.CreateDefaultBuilder() is used to return an IWebHost pre-configured with the defaults necessary to run as an IntegratedIISInProcess WebHost.
launchSettings.json refers to IISExpress
Details in [Demo02 Server](Server/ReadMe.html)

