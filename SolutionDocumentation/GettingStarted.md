# Getting started with the Blazor - ServiceStack Demos (at the SolutionDocumentation subfolder level)
If you are viewing this GettingStarted.md in GitHub, [here is this same document on the documentation site](GettingStarted.html)

These are detailed instructions for getting started building, running and extending these demonstrations

## <a DocumentationOrganization/> Documentation organization
[Details on the documentation and code organization](Organization.html) will provide details on how the code for these demos is organized, and details on what documentation is available at each level of the code.

## <a Prerequisites/> Prerequisites

1. A C# / .NET  development environment capable of supporting Blazor development
    The documentation is currently written only for Visual Studio V15.9 (or higher) IDE. Pull requests that expand the documentation to cover the command-line MSBuild environment or other IDEs would be most welcome!
	The specific instructions to extend the VS IDE to build Blazor apps are in the [Get started with Blazor](https://blazor.net/docs/get-started.html) documentation, so head over there and make sure you have the necessary .Net Core 2.1 SDK and then install the Blazor bits
	[optional 3rd-party extensions and tools]() is a link to instructions on some specific 3rd party tools that make it easier to debug these demos, or see what's going on under the hood
	     a. [Fiddler]() - to see what is being sent between the Blazor app and the ServiceStack Host
		 b. [Sentinel]() - a logging target that listens on a UDP port and displays log messages
		 c. [Structured Build Log Viewer]() - a viewer to see details of how MSBuild is building the assemblies.

## Getting the Demo projects
    This repository is not yet sophisticated enough to create a zip with binaries and be able to safely secure and deliver that. Besides, building these demos from source is probably the best way to understand what's going on. Your current choices are to fork or clone this repository. GitHub has a nice explanation of the differences, as well as [detailed instructions on how to fork or clone](https://github.community/t5/Support-Protips/The-difference-between-forking-and-cloning-a-repository/ba-p/1372). Start by forking or cloning your own copy the repository, and then and attach your Visual Studio Source Code Control provider to the fork.

## Building the Demo projects
There is documentation that goes into details for [building the demos here](BuildingNotes.html). Below is a high-level description of what needs to be done.
1. Open the top level .sln file with Visual Studio
1. Tell Visual Studio to not build the documentation project *ATAP.Utilities.AutoDoc*, as it will take quite a bit of time, and is not needed to run the demos. [How to: Exclude projects from a build](https://docs.microsoft.com/en-us/visualstudio/ide/how-to-exclude-projects-from-a-build?view=vs-2017)
1. Build the entire solution in the Debug configuration.
1. Go into each individual Demo, and "Publish" the GUI.
1. Start the SS Console App
1. Open a browser and navigate to http://localhost:21200 as this is the default address and port that the ConsoleApp will be listening on..

## Running each Demo project
Each demo tries to make a short, focused point. In the GUI, this means only a very few pages with which to interact. The individual demo's documentation should make it clear how to interact with each page and what to expect.

## Contributing
Pull requests for collaborating on and extending these demonstrations are most welcome! Please see the [the Contributing guidelines](Contributing.html) for further details.
Contributing to the documentation for these examples is welcome as well. There is no written documentation on how to accomplish that yet, but please refer to the incredibly well written document [Contributing to the Blazor documentation](https://github.com/aspnet/Blazor.Docs/blob/master/CONTRIBUTING.md) for ideas. Our Folder Structure conventions differ from the examples in that document.

## Attribution
The maintainers and contributors to this repository feel it is important to credit the individuals and organizations who have taken their time to publish ideas and guides. We have two documents that try to provide credit where it is due:
1. [Attribution for guides and articles related to Blazor and ServiceStack](Attribution.html)
1. [Attribution for guides and articles related to the Utility assemblies, related to DocFx, and related to design, building, and debugging code in general](https:///GitHub.com/BillHertzing/Atap.Utilities/SolutionDocumentation/Attribution.html)
