# Blazor With ServiceStack Demonstrations ReadMe (at the common Demos Solution subfolder level)
If you are viewing this ReadMe.md in GitHub, ~~[here is this same ReadMe on the documentation site](ReadMe.html)~~

The demos in this repository are currently using Blazor Version .Net Core 3.0 Preview 5.

## Common Documentation for all Demos
Information here relates to all of the demos. At the bottom of this page is a [list of the demos and what each does]~~(./ReadMe.html#DemoList)~~~

## Prerequisites
Refer to [Getting Started]~~(../SolutionDocumentation/GettingStarted#prerequisites)~~ for detailed information on the following necessary prerequisites.

1. Visual Studio (VS) 2019 Version 116.1 or newer. All of the following instructions assume you are using a Visual Studio (VS) 2019 IDE for development. The section below on [Building]~~(./ReadMe.html#Building)~~ does provide references to  additional documentation on options for building the demos outside of Visual Studio.
1. Familiarity with using Git and GitHub in VS.
1. ServiceStack (SS) Version 5.*. You will need to refer to [Instructions for adding ServiceStack via NuGet into a solution](https://servicestack.net/download). Unless you purchase a license, SS will be the "Starter" version, limited to about 10 REST service endpoints Each of the demonstration programs here will be written to stay below the limit. 
    * You should also be aware that the ServiceStack development team does a great job of patching and enhancing ServiceStack, and there may be times you will want to get new patches from ServiceStack's MyGet feed. You will want to go to VS's Tools -> Options -> NuGet Package Manager -> Package Sources and add to the "Available package sources". Add https://www.myget.org/F/servicestack to the list of package sources.		
1. Blazor components installed into VS. Instructions for getting Blazor setup for VS can be found here: https://blazor.net/docs/get-started.html. Blazor is changing rapidly, and I will do my best to ensure that the demos in this repository track the changes in Blazor.
1. A logging framework such as the Open Source Software (OSS) NLog installed into VS. A good post explaining how to integrate NLog with VS can be found here: https://www.codeguru.com/csharp/csharp/cs_network/integrating-nlog-with-visual-studio.html. The demos here use NLog at the beginning of the series for the simple . Other logging providers are introduced later, as well as advanced logging for specific audiences. The NLog configuration file included in these demos also specifies a UDP-based logger. **Sentinel**, described below, is a good-enough choice for a UDP-based log viewer application.
1. Blazor logging framework. Source and ReadMe.md for the extensions can be found here: https://github.com/BlazorExtensions/Logging. A good post explaining how to use the extension in your Blazor project can be found here: https://www.c-sharpcorner.com/article/introduction-to-logging-framework-in-blazor-with-net-core/. 
 
# Building the demos <a id="Building"/>
All of these instructions refer back to the `Atap.Utilities` repository's documentation that provides my most up-to-date instructions on building solutions in these repositories.
  * [Building a solution from Visual Studio]()
  * [Building a solution Using MSBuild via a Command Line Interface (CLI)]()
  * [Building a solution using the DotNet build command]()

## Publishing the demos

## Publishing the Blazor GUI
The Blazor GUI always requires an additional publishing step beyond just building the application. In all of these demos, we will use the following architecture for the base location where the Publish step will put the files that make up the Blazor GUI. Blazor GUI. In each Demo, under the GUI project's Properties subfolder, are the two files `DebugFolderProfile.pubxml` and `ReleaseFolderProfile.pubxml`. These files have the property **\<publishUrl>** which controls the location to which the GUI project is published. The Publish action creates a subfolder path *\<ProjectName>\dist* under the location specified in the **\<publishUrl>**. I wanted it to be easy to delete the published GUI files during development, so I decided to put the Publish location underneath the GUI project's **\<OutputDir>**. Since the **\<publishUrl>** is relative to the **\<ProjectDir>**, I hardcoded the path I wanted for Debug builds (**\<OutputDir>** for Debug and **\<TargetFramework netstandard2.0**), making it *bin\Debug\netstandard2.0\Publish*. The complete path of the published GUI will be *bin\Debug\netstandard2.0\Publish\GUI\dist*. After you have done a Publish of the GUI project, use a file explorer to verify the contents of the *\<GUIProjectDir>\bin\Debug\netstandard2.0\Publish\GUI\dist*. You will find the static files that make up the Blazor app, including the wwwroot static content and the multitude of DLLs.

## Publishing the Server
At a certain point in this series, the `Server` project moves out of development subdirectories and into testing, staging, and production direcotries. This requires a series of build steps that include / start with the `Publish` operation. Details on this are provided in the ReadMe for the specific demos where this occurs.

# Running and debugging the demos
  * [Running a ServiceStack ConsoleApp from within Visual Studio]()
  * [Running a ServiceStack ConsoleApp from via a Command Line Interface (CLI)]()
  * [Running a ServiceStack ConsoleApp via the DotNet run command]()


# <a id="DemoList"/>A List of the Demos

    1. Demo01 : [Blazor GUI served by ServiceStack](Demo01/ReadMe.html)
  
      The first demonstration program is the most basic. The Blazor GUI portion consists of one simple Razor page with no code, and code that makes two REST calls to ServiceStack, one REST call with no data payload sent or received, and one that sends a string and receives a string. The ServiceStack application portion consist of a Console program for .Net (full framework) which serves the static files for the Blazor application, and handles the two simple REST service endpoints. The CommonDTOs project defines the data payload sent and received between the ConsoleApp and the Blazor GUI. Added in V0.1.0
	  Blazor GUI targets .Netstandard 2.0, and will do so for all demos
	  ConsoleApp targets .Net 4.0
	  SS is hosted by the builtin Windows HttpListener over the Window's kernel-level http.sys.
	  ServiceStack derives from `AppSelfHostBase`
	  Logging is via SS's logging provider coupled with NLog, writing via UDP to a UDP listener, and will stay this way in the demo series until Demo09

    1. Demo02 : [Blazor GUI with an async OnInit call to the SS middleware served with a default Web Host (IISIntegratedInProcess)]

	  The GUI adds logging via the Blazored.Logging package. It adds a codebehind page to the single razor page, and provides a delegate for the `OnInit` event of the Razor page's lifecycle. The OnInit delegate in Demo02 makes a REST call via extension methods on the injected HTTPClient. No data is sent or received. The URL for the REST call is hardcoded in the `OnInit` delegate. ServiceStack on the server will intercept this request based on the path portion of the URL, and generate a response.  The GUI will read the response, but do nothing with it beyond logging it.
	  
	  The `CommonDTOs` project is used for the first time. Request and Response Data Transfer Objects (DTOs) are defined. The first DTOs are for the HTTP call made within the `OnInit` event handler in the GUI. The CommonProjects assembly targets .netstandard 2.0, is referenced by and included in the GUI Blazor payload.

	  The `ConsoleHost` subdirectory becomes the`Server` directory. The project is likewise renamed, and the target framework is now .Net Core V2.0. A static method to create a WebHostBuilder makes its appearence, named `CreateIntegratedIISInProcessWebHostBuilder`, and a webHost is instanced when the builder .Build(). The static uses 
	    the CreateDefaultBuilder builder extension method. This means IISIntegratedInProcess was added to the builder's webhost configuration object, and that the webHost built from this static method will always follw the model of be IISIntegratedInProcess
	   ServiceStack now derives from `AppHostBase`, and AppHost.cs becomes SSAppHost.cs. The CommonProjects assembly is referenced by and included in Server.
	  The second demonstration program expands on the adds a codebehind page j umps through hoops to demonstrate that the SS TextUtils can work within the client side Blazor. The .Dump() extension is used to prettyprint complex data objects to a string, and puts it to both the console log and to the Demo02 page. The serializers and deserializers convert complex data objects to / from a string, and uses the built-in HttpClient to send/receive the strings. Added in V0.2.0.
	
	with ServiceStack TextUtils, using the .Dump() method and the JSON serializer and Deserializer, round-tripping a omplexData object (POCO), and a dictionary of these ComplexData objects]
      
	  	  ConsoleApp targets .Net 4.0
	  SS is hosted by the builtin Windows HttpListener over the Window's kernel-level http.sys'


    1. Demo03 : [Blazor GUI with ServiceStack C# HttpClient-based JsonHttpClient library]
      
	   The third demonstration program demonstrates that the SS C# HttpClient-based JsonHttpClient library can work within the client- side Blazor as a replacement for the HttpClient service that Blazor supplies out of the box. Added in V0.3.0.

	1. Demo04 : [Blazor GUI with ServiceStack C# Client]
      
	   The fourth demonstration program demonstrates using some of the SS JsonHttpClient library's features including (TBD). Added in V0.4.0.

	   Later demos 
	   using ServiceStack's JSON Serializer and Deserializer
	   using ServiceStack's HttpClient-based JsonHttpClient library
	   using ServiceStack's C# Client
	   using ServiceStack's SSE features

