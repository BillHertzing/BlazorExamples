# Blazor With ServiceStack Demonstrations Demo05 ReadMe (at the Demo05 subfolder level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

This is the specific documentation for Demo05, *Blazor GUI served by ServiceStack Middleware hosted by selectable WebHost hosted by a GenericHost (.Net Core V3.0)*.

## Introduction
The Blazor GUI project ...
The server project moves to Dot Net Core V3.0 and adds Environment and 'Development-environment-only' features
The Common DTOs ..

## Blazor GUI
The .sccs file adds styles for AnIntegerProperty, and IncrementAnIntegerPropertyButton (for both Active and queuing visual effects)
The GUI adds a timer that will increment AnIntegerProperty, a button to start/stop the timer, styles for the TimerControl button, and state triggers for the button and for the timer itself

Details in [Demo05 Blazor GUI](GUI/ReadMe.html)

## Server
The server targets .Net Core V3.0 and will remain under V3.0 for the rest of the demo series
Environment Variables and LaunchSettings.json grow to add Environment as an Environment Variable.
The genericHostBuilder is modified after its creation to add "nice-to-have" developer capabilities when debugging under the Development Environment

Details in [Demo05 Server](Server/ReadMe.html)


