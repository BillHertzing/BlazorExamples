# Examples of Blazor Apps served By ServiceStack

Blazor is an experimental technology from Microsoft that allows applications written in C# to run on any browser that supports WASM. Many other folks have written better introductions and explanations of Blazor and WASM than I can, so please search the Web for those terms if you would like detailed background information on this emerging technology.

Blazor applications can run server-side (Server-Side Blazor, or SSB), or client-side (Client-Side Blazor, or CSB). The examples in this repository are all about client-side Blazor. The files needed to run a Blazor application can be served to a browser by any process that understands HTTP and can serve static files. Of course, all the popular web server packages can do this, as can many Cloud services. There are many resources on the web that can go into much greater detail for using those technologies.

However, I've been looking for a way to leverage the browser as a GUI for an application that runs on multiple operating systems. Most application that need a GUI have to create the GUI specifically for an OS. Blazor brings the ability for developers to write their GUI in Razor and C#, publish it to a set of static files, and have any browser render and run the GUI. An application that leverages .Net, .Net Standard, and .Net Core to run on multiple OSs, combined with a Blazor-based GUI, promises to greatly reduce the platform-specific portions of any multi-OS application.

ServiceStack is a very popular product that provides REST endpoints for an application. ServiceStack can also serve static files. Combining the two, ServiceStack can serve the files needed for a Blazor GUI, and can also serve the REST endpoints that allow the GUI to communicate with the application. 

This repository will focus specifically on using the ServiceStack application to serve and interact with a Blazor application. The contents of this repository will be demonstration programs showing how to integrate Blazor with various ServiceStack features. These examples are simplified versions of the ACE application and it's Blazor GUI, which is in the Ace repository adjacent to this one.

## Example 1

The first example program is the most basic. The Blazor GUI portion consists of two Razor pages, and code that makes two REST calls to ServiceStack, one REST call with no data payload sent or received, and one that sends a string and receives a string. The ServiceStack application portion consist of a Console program for .Net (full framework) which serves the static files for the Blazor application, and handles the two simple REST service endpoints. 

## Prerequisites
1. Visual Studio 2017 Version 15.8 or newer. All of the following instructions assume you are using a Visual Studio (VS) 2017 IDE for development, and are pretty familiar with using Git and GitHub in VS.
1. ServiceStack (SS) Version 5.4.1. Instructions for adding ServiceStack via NuGet into a solution can be found here: https://servicestack.net/download. Unless you purchase a license, SS will be the "Starter" version, limited to about 10 REST service endpoints Each of the demonstration programs here will be written to stay below the limit. 
    * You should also be aware that the ServiceStack development team does a great job of patching and enhancing ServiceStack, and there may be times you will want to get new patches from ServiceStack's MyGet feed. You will want to go to VS's Tools-> Options -> NuGet Package Manager -> Package Sources and add to the "Available package sources". Add https://www.myget.org/F/servicestack to the list of package sources.		
1. Blazor 0.6.0 components installed into VS. Instructions for getting Blazor setup for VS can be found here: https://blazor.net/docs/get-started.html. Blazor is changing rapidly, and I will do my best to ensure that the examples in this repository track the changes in Blazor.
1. A logging framework such as the Open Source Software (OSS) NLog installed into VS. A good post explaining how to integrate NLog with VS can be found here: https://www.codeguru.com/csharp/csharp/cs_network/integrating-nlog-with-visual-studio.html. The examples here use NLog. The NLog configuration file included in these examples also specifies a UDP-based logger. Sentinel, described below is a good choice for a UDP-based logging application.
1. Blazor logging framework. Source and ReadMe.md for the extensions can be found here: https://github.com/BlazorExtensions/Logging. A good post explaining how to use the extension in your Blazor project can be found here: https://www.c-sharpcorner.com/article/introduction-to-logging-framework-in-blazor-with-net-core/. The examples in this repository are currently using Version 0.9.0. I will do my best to ensure that the examples in this repository track the changes in the Blazor logging extensions.

## Suggested but not required
1. The free UDP logging application Sentinel V0.13.0.0, which can be installed to Windows from here: https://github.com/yarseyah/sentinel.
1. Telerik's  free Fiddler 4 product or equivalent for monitoring the HTTP traffic between the browser and the ServiceStack instance, which can be installed from here:  https://www.telerik.com/download/fiddler.

## Getting Started
1. Install the prerequisites listed above onto your development computer.
1. Install and configure the two monitoring tools, Sentinel and Fiddler, if desired.
### Getting Example 1
1. Get a copy of the example's source code. 
  * You may do this by forking this repository into your own repository, and connecting VS's Team Explorer on you development computer to the new remote repository, and making a local branch of the remote repository on your development computer. This is great if you want to play with the example under version control.
  * You may just want to download a zip of the source code from GitHub and expand it on your local development computer, and work with it disconnected from Git version control.
### Compiling and Publishing Example 1  
1. Open the Solution file (.sln) with VS.
  * The solution file describes the three individual projects and the solution folders, and the local branch of the fork (or extracted zip) will create the physical subdirectories that correspond to the layout of the project in the solution file.
1. Ensure the build configuration at the solution level is `Debug`, that the build configuration trickles down to all three projects.
1. right-click the solution  in Solution Explorer, and click "Build Solution".
1. right-click the ConsoleApp project in Solution explorer, and select "Set as Startup Project".
1. right-click the GUI project, and select "Publish...". On the GUI Publish page that appears, ensure the Profile dropdown is displaying `DebugFolderProfile`, then press the `Publish` button.

Before running the example, I suggest you get the monitoring tools up and running. These are not required, but they certainly make it much easier to see what the programs are doing. Instructions for doing so are further down in this document, under "Starting the Monitoring tools".

## Run the Example
1. Press F5 key in VS, and the ConsoleApp will start under the debugger. Running the Console App under the VS debugger provides all the usual VS debugger goodness, so you will probably want to start the ConsoleApp with F5 most of the time. If all goes well, shortly after pressing F5, a console window will appear above (obscuring part of) VS, with the ConsoleApp's welcome message. Just leave the window up. As the ConsoleApp runs, it will print log messages to this window (as well as to Sentinel). Pressing any key in this console window will end the application. Pressing F5 again will start it again.
2. Browse to the port that ServiceStack is ListeningOn
   1. Bring up a browser. Whatever browser you please, as long as it is modern enough to run WASM. If you are interested in this article, you probably keep the browser on your development computer pretty recent. 
   2. Navigate the browser to the ConsoleApp's listening URL (http://locahost:21200) as configured in this example. You should see the home page of the example appear in your browser, and Fiddler should show you a lot of traffic as ServiceStack delivers to the browser all the files requested by the Blazor app, both normal CSS content, and all the DLL files too.

### Standard Edit/Compile/Debug cycle for these examples goes like this.
* After making editing changes to the GUI, publish it.
* After making editing changes to the ConsoleApp, press F5 to start it under the debugger. 
* If you make changes to both, be sure to `Publish` the GUI before building/debugging the ConsoleApp.
* Open a browser and type in the network address where the Console App is listening ServiceStack, "http://localhost:22100"
* Look at the Fiddler and Sentinel windows, and the browser console, and correlate the log messages there.
* Use VS's debugger to set breakpoints and examine code and data in the ConsoleApp.
* The client-side WASM app in the bowser doesn't have debugger support yet, but that will change eventually, and I hope to keep these examples updated. For now, debugging is via the old-fashioned way, log message tracing. Blazor log messages go to the browser's Console window, which can be viewed in the browser's Developer tools. The normal way to display the Developer tools in a browser is to press F12.

# How to make ServiceStack deliver the GUI's static files
Being able to tell the ConsoleApp where the static files are located is a key requirement. In the Console Apps' AppHost.cs file, is a line that specifies it. The solution shown here is specific to the way VS uses MSBuild and it's $OutputDir properties during builds
In the Console Apps' AppHost.cs file, is a line 
```C#
#code var physicalRootPath = "../../../../GUI/bin/Debug/netstandard2.0/Publish/GUI/dist";
```
Use a file explorer to ensure the GUI's content, including the wwwroot static content and the multitude of DLLs, are published to this location. The file DebugFolderProfile.pubxml has a property <publishUrl>bin\Debug\netstandard2.0\Publish</publishUrl> which controls the location. It is important that this align with the physicalRootPath string constant in the Console Apps's Virtual to Physical File Mapping call.

Details on the physicalRootPath value.

By default VS puts the compile/link artifacts under a projects $OutputDir, which defaults to ./bin/<Config>/<framework> subfolder relative to the $ProjectDir
Under VS debugging, the ConsoleApp, also starts the .exe in that same $OutputDir /bin/<Config>/<framework> subfolder. So to map from the apps directory to the Blazor apps files: the four ../../.././ patterns maps to the MSBuild $SolutionDir top-level folder, just above each $ProjectDir. Then the Blazor app's static files are down the path $ProjectDir (GUI), then bin/<Config>/<Framework>, and then the location specified in the DebugFolderProfile.pubxml.  Putting it all together, the Console App knows where the WASM static files are, by going up to the $SolutionDir, then down the path to the $OutputDir where MSBuild places all the DLLs produced by the GUI Builds, and then down into the Publish/GUI/dist subfolder created by Publish.
  The solution above is very specific to the way MSBuild works, and that specific convoluted physicalRootPath is specific to the way the example project structure was organized. In production or other scenarios, the var physicalRootPath would have a different value..

### Starting the Monitoring tools
1. Start Fiddler, ensure it is listening to all processes. There will be a lot of cruft in the window, hundreds of request/response pairs from all the browser windows you probably have open on your development computer. It takes a while working with Fiddler to setup filters that eliminate all the other HTTP traffic coming and going in your computer, until you can see just the Blazor and ServiceStack traffic.
2. Start Sentinel, and go through its startup screens to setup the UDP listener, which will be listening for logging messages broadcast to its default listening port. This would also be a good time to inspect the NLog.config file in the example. You will see that it sends all messages from any class to two loggers, the Console logger (for the ConsoleApp's console window), and to the UDP logger as well. So for this example, the Sentinel logging program is not 100% necessary, but it will be necessary later, when ServiceStack is running in a mode that has no console (called headless mode). Getting it setup and running also makes development much easier, as the log message don't all disappear as soon as the program stops.

If you would like to save some keystrokes, Visual Studio can be confireud to start your browser and navigate to a URL when your press F5. This can be controlled by the launchSettings.json file. the launchSettings.json file is used by Visual Studio to launch your application and controls what happens when you hit F5.  In this example, the launchSettings.json file is found under the Properties subfolder of the ConsoleApp's $ProjectDir'
Pressing F5 and have it bring up a browser tab for you automatically. This is controlled by the .launchsettings.xml file found under the Properties Subfolder in the ConsoleApp.
The launchSettings.json file is used by Visual Studio to launch your application and controls what happens when you hit F5. 

STOPPING point


The AceAgent Blazor GUI provides a means for humans to interact with the AceAgent. The AceAgent Blazor GUI is built like any Blazor app, and is published to a location in the File system. The GUIServices PlugIn configures the AceAgent to properly serve the static files needed to load and route the Blazor app on any browser.


## Projects/Assemblies
### CommonDTOs
Class the define the format of teh Request and response data paayloads. These classes are used by both the Blazor app and teh ServiceStack App. They are in a seperate Assembly that is built against bothteh .net 471 and .NetStqndard frameworks.
Class the define the format of teh Request and response data paayloads. These classes are used by both the Blazor app and teh ServiceStack App. They are in a seperate Assembly that is built against bothteh .net 471 and .NetStqndard frameworks.
For details on the Blazor GUI application, look at the project Ace.AceGUI. For details onthe AceAgent applicationthat serves the aceGUI static files and provides API endpoints, see thesection Ace.AceAgent.
### GUI
Most people who come here for the Blazor examples will be interested primarly in this project/assembly. This Blazor GUI App is derived from the stock examples found on GitHub.
###Pages
####BIndex
Display a simple home.
####BaseServices
Display a simple page that interfaces with the APIs provided in the base services of the AceAgent.


This example only has the Windows version, called AceService. 
This assembly contains the main entry point into the agent. It is written using the ServiceStack framework (https://servicestack.net/). it  runs as a Console App. 
#### Logic
It configures ServiceStack behavior as follows:

* It instructs ServiceStack to generate CORS headers.
  * setting the port AceAgent listens on.


##### Logic 
-setting the root of the virtualpath to either "" or some other string (should be an string that is acceptable as a URL virtual path root).
-specifying a path on the computer's file system from which to serve static files.
-It instructs ServiceStack to respond to requests on the virtual path with a file name if the Request's URL matches a file name found in the virtualpath. It maps subdirectories of the physical path to subdirectories of the virtual path, and delivers files from these locations as well.
-It instructs ServiceStack to respond to requests on the virtual path with a redirect to virtualpath/index.html, if the request on the virtual path does not match any file name.


## Notes on Building
- AceGUI must target netstandard2.0
- AceService must target net47
- Any assembly with .PlugIn: must target net47
- Any assembly with .Models that are used by both the server and client side, must target both net47 and netstandard2.0
- Any Assembly with .UnitTests must target net47
