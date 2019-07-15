# Blazor With ServiceStack Demonstrations Demo02 ReadMe (at the Demo02 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo02, *Blazor GUI served by ServiceStack Middleware hosted in an Integrated IIS In-Process WebHost (.Net Core V2.1)*.

## Introduction
The Blazor GUI project introduces page-local properties, a button, and the button's synchronous OnClick handler 
The server project targets Net Core V2.1. An IntegratedIISInProcess WebHost is introduced.

## Blazor GUI
The `index` page implements a page-local property `AnIntegerProperty`.
The `index` page implements the `IncrementAnIntegerPropertyButton` onclick events`<button>` element that references an `OnClick` handler.
The GUI implements a simple synchronous `OnClick` handler which increments the value of the `AnIntegerProperty`.

## GUI Styling
A background color is added to the site.css file. The background color will be changed in each Demo to provide a visible difference in each Demo

Details in [Demo02 Blazor GUI](GUI/ReadMe.html)

## Server
The Server moves the SSApp middleware into an IntegratedIISInProcess WebHost running under .Net Core V2.1.
A static method for the creation of an IWebHostBuilder configured to create an IntegratedIISInProcess WebHost is introduced.
Within the static method, the extension method WebHost.CreateDefaultBuilder() is used to return an IWebHost pre-configured with the defaults necessary to run as an IntegratedIISInProcess WebHost.
launchSettings.json refers to IISExpress

Details in [Demo02 Server](Server/ReadMe.html)

