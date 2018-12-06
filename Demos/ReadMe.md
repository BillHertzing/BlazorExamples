# Common Documentation for all Demos
Information here relates to all of the demos. At the bottom of this page is a [list of the demos and what each does](./ReadMe.html#DemoList)
# Prerequisites
1. Visual Studio (VS) 2017 Version 15.8 or newer. All of the following instructions assume you are using a Visual Studio (VS) 2017 IDE for development. The section below on [Building](./ReadMe.html#Building) does reference additional documentation on options for building the demos outside of Visual Studio
1. Familiarity with using Git and GitHub in VS.
1. ServiceStack (SS) Version 5.*. You will need to refer to [Instructions for adding ServiceStack via NuGet into a solution](https://servicestack.net/download). Unless you purchase a license, SS will be the "Starter" version, limited to about 10 REST service endpoints Each of the demonstration programs here will be written to stay below the limit. 
    * You should also be aware that the ServiceStack development team does a great job of patching and enhancing ServiceStack, and there may be times you will want to get new patches from ServiceStack's MyGet feed. You will want to go to VS's Tools -> Options -> NuGet Package Manager -> Package Sources and add to the "Available package sources". Add https://www.myget.org/F/servicestack to the list of package sources.		
1. Blazor components installed into VS. Instructions for getting Blazor setup for VS can be found here: https://blazor.net/docs/get-started.html. Blazor is changing rapidly, and I will do my best to ensure that the demos in this repository track the changes in Blazor.
1. A logging framework such as the Open Source Software (OSS) NLog installed into VS. A good post explaining how to integrate NLog with VS can be found here: https://www.codeguru.com/csharp/csharp/cs_network/integrating-nlog-with-visual-studio.html. The demos here use NLog. The NLog configuration file included in these demos also specifies a UDP-based logger. **Sentinel**, described below, is a good choice for a UDP-based logging application.
1. Blazor logging framework. Source and ReadMe.md for the extensions can be found here: https://github.com/BlazorExtensions/Logging. A good post explaining how to use the extension in your Blazor project can be found here: https://www.c-sharpcorner.com/article/introduction-to-logging-framework-in-blazor-with-net-core/. The demos in this repository are currently using Version 0.9.0. I will try to ensure that the demos in this repository track the changes in the Blazor logging extensions.
 
# <a id="Building"/>Building the demos
All of these instructions refer back to the `Atap.Utilities` repository's documentation that provides my most up-to-date instructions on building solutions in these repositories.
  * [Building a solution from Visual Studio]()
  * [Building a solution Using MSBuild via a Command Line Interface (CLI)]()
  * [Building a solution using the DotNet build command]()
## Building and Publishing the Blazor GUI
The Blazor GUI requires an additional publishing step beyond just building the application. In all of these demos, we will use the following architecture for the base location where the Publish step will put the files that make up the Blazor GUI. Blazor GUI. In each Demo, under the GUI project's Properties subfolder, are the two files `DebugFolderProfile.pubxml` and `ReleaseFolderProfile.pubxml`. These files have the property **\<publishUrl>** which controls the location to which the GUI project is published. The Publish action creates a subfolder path *\<ProjectName>\dist* under the location specified in the **\<publishUrl>**. I wanted it to be easy to delete the published GUI files during development, so I decided to put the Publish location underneath the GUI project's **\<OutputDir>**. Since the **\<publishUrl>** is relative to the **\<ProjectDir>**, I hardcoded the path I wanted for Debug builds (**\<OutputDir>** for Debug and **\<TargetFramework netstandard2.0**), making it *bin\Debug\netstandard2.0\Publish*. The complete path of the published GUI will be *bin\Debug\netstandard2.0\Publish\GUI\dist*. After you have done a Publish of the GUI project, use a file explorer to verify the contents of the *\<GUIProjectDir>\bin\Debug\netstandard2.0\Publish\GUI\dist*. You will find the static files that make up the Blazor app, including the wwwroot static content and the multitude of DLLs.

# Running and debugging the demos
  * [Running a ServiceStack ConsoleApp from within Visual Studio]()
  * [Running a ServiceStack ConsoleApp from via a Command Line Interface (CLI)]()
  * [Running a ServiceStack ConsoleApp via the DotNet run command]()


# <a id="DemoList"/>A List of the Demos

  1. Demo01 : [Blazor GUI served by ServiceStack having two REST endpoints](Demo01/ReadMe.html)
  
    The first example program is the most basic. The Blazor GUI portion consists of two Razor pages, and code that makes two REST calls to ServiceStack, one REST call with no data payload sent or received, and one that sends a string and receives a string. The ServiceStack application portion consist of a Console program for .Net (full framework) which serves the static files for the Blazor application, and handles the two simple REST service endpoints. The CommonDTOs project defines the data payload sent and received between the ConsoleApp and the Blazor GUI.


