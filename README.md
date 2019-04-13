# Blazor With ServiceStack Demonstrations ReadMe (at the repository root / solution root Solution Item level)
If you are viewing this ReadMe.md in GitHub, [here is this same ReadMe on the documentation site](ReadMe.html)

Blazor is an experimental technology from Microsoft that allows applications written in C# to run on any browser that supports WASM. Here is where you can find [Blazor's Getting Started page](https://blazor.net/docs/get-started.html). Many other folks have written better introductions and explanations of Blazor and WASM than I can, so please search the Web for those terms if you would like detailed background information on this emerging technology. 

Blazor applications can run server-side (Server-Side Blazor, or SSB), or client-side (Client-Side Blazor, or CSB). The demos in this repository are all about client-side Blazor. The files needed to run a Blazor application can be served to a browser by any process that understands HTTP and can serve static files. Of course, all the popular web server packages can do this, as can many Cloud services. There are many resources on the web that can go into much greater detail for using those technologies.

However, I've been looking for a way to leverage the browser as a GUI for an application that runs on multiple operating systems. Most application that need a GUI have to create the GUI specifically for an OS. Blazor brings the ability for developers to write their GUI in Razor and C#, publish it to a set of static files, and have any browser render and run the GUI. An application that leverages .Net, .Net Standard, and .Net Core to run on multiple OSs, combined with a Blazor-based GUI, promises to greatly reduce the platform-specific portions of any multi-OS application.

ServiceStack is a very popular product that provides REST endpoints for an application. ServiceStack can also serve static files. Combining the two, ServiceStack can serve the files needed for a Blazor GUI, and can also serve the REST endpoints that allow the GUI to communicate with the application. 

This repository will focus specifically on using the ServiceStack application to serve and interact with a Blazor application. The contents of this repository will be demonstration programs showing how to integrate Blazor with various ServiceStack features. These demos are simplified versions of the *ACE* application and it's Blazor GUI, which is in the *Ace* repository adjacent to this one. You will also find that the documentation here may refer to documentation found in the *ATAP.Utilities* repository, for further information regarding utilities  written to aid in the building and debugging of applications using Visual studio.

# Getting Started
Currently the only way to use these demos is to fork or clone the repository and build the demos. Here are some instructions on [how to fork a GitHub repository](https://help.github.com/articles/fork-a-repo/)

# Building, Running, and Debugging these demos
The [Common Documentation for all Demos]() has a section for [Building, Running, and Debugging]() 


## Example 1

The first example program is the most basic. The Blazor GUI portion consists of two Razor pages, and code that makes two REST calls to ServiceStack, one REST call with no data payload sent or received, and one that sends a string and receives a string. The ServiceStack application portion consist of a Console program for .Net (full framework) which serves the static files for the Blazor application, and handles the two simple REST service endpoints. 

## Prerequisites
1. Visual Studio 2019 Preview Version 16.0.0 or newer. All of the following instructions assume you are using a Visual Studio (VS) 2017 IDE for development, and are pretty familiar with using Git and GitHub in VS.
1. ServiceStack (SS) Version 5.5.0. Instructions for adding ServiceStack via NuGet into a solution can be found here: https://servicestack.net/download. Unless you purchase a license, SS will be the "Starter" version, limited to about 10 REST service endpoints Each of the demonstration programs here will be written to stay below the limit. 
    * You should also be aware that the ServiceStack development team does a great job of patching and enhancing ServiceStack, and there may be times you will want to get new patches from ServiceStack's MyGet feed. You will want to go to VS's Tools-> Options -> NuGet Package Manager -> Package Sources and add to the "Available package sources". Add https://www.myget.org/F/servicestack to the list of package sources.		
1. Blazor 0.9.0 components installed into VS. Instructions for getting Blazor setup for VS can be found here: https://blazor.net/docs/get-started.html. Blazor is changing rapidly, and I will do my best to ensure that the examples in this repository track the changes in Blazor.
1. .NET Core 3.0 Preview 3 SDK (3.0.100-preview3-010431) installed onto the development computer, and Visual Studio 2019 configured to use preview SDKs. Instructions for loading the .NET Core 3.0 Preview 3 SDK can be found here: https://dotnet.microsoft.com/download/dotnet-core/3.0. To configure VS 2019 to use the preview version, go to Tools -> Options -> Projects and Solutions -> .NET Core, and check the box "use previews of the .NET Core SDK"
1. A logging framework such as the Open Source Software (OSS) NLog installed into VS. A good post explaining how to integrate NLog with VS can be found here: https://www.codeguru.com/csharp/csharp/cs_network/integrating-nlog-with-visual-studio.html. The examples here use NLog. The NLog configuration file included in these examples also specifies a UDP-based logger. Sentinel, described below is a good choice for a UDP-based logging application.
1. ~~Blazor logging framework. Source and ReadMe.md for the extensions can be found here: https://github.com/BlazorExtensions/Logging. A good post explaining how to use the extension in your Blazor project can be found here: https://www.c-sharpcorner.com/article/introduction-to-logging-framework-in-blazor-with-net-core/. The examples in this repository are currently using Version 0.9.0. I will do my best to ensure that the examples in this repository track the changes in the Blazor logging extensions.~~ As of 4/13/19, the Blazor Logging Framework, both V0.9.0 and V0.10.0, reference Blazor V0.7.0. This causes an incompatability. Logging from teh C# code on the browser is currently "commented out" in all examples. I try to update this when the Blazor logging framework is updated.

## Suggested but not required
1. The free UDP logging application Sentinel Version 0.13.0.0 or equivalent, which can be installed to Windows from here: https://github.com/yarseyah/sentinel.
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
2. Browse to the port that the ConsoleApp is configured to ListeningOn
   1. Bring up a browser. Whatever browser you please, as long as it is modern enough to run WASM. If you are interested in this article, you probably keep the browser on your development computer pretty recent. 
   2. Navigate the browser to the ConsoleApp's listening URL (http://locahost:21200) as configured in this example. You should see the home page of the example appear in your browser, and Fiddler should show you a lot of traffic as ServiceStack delivers to the browser all the files requested by the Blazor app, both normal CSS content, and all the DLL files too.

### Standard Edit/Compile/Debug cycle for these demos goes like this.
* After making changes to the GUI, publish it, which will build as a first step.
* After making changes to the ConsoleApp, press F5 to start it under the debugger, which will build as a first step. 
* If you make changes to both, be sure to `Publish` the GUI before building/debugging the ConsoleApp.
* Open a browser and type in the network address where the Console App is listening ServiceStack, "http://localhost:22100"
* Look at the Fiddler and Sentinel windows, and the browser console, and correlate the log messages there.
* Use VS's debugger to set breakpoints and examine code and data in the ConsoleApp.
* The client-side WASM app in the bowser doesn't have debugger support yet, but that will change eventually, and I hope to keep these demos updated. For now, debugging is via the old-fashioned way, log message tracing. Blazor log messages go to the browser's Console window, which can be viewed in the browser's Developer tools. The normal way to display the Developer tools in a browser is to press F12.

# How to make ServiceStack deliver the Blazor app
You will need to start with version  5.4.1 or higher, because ServiceStack developers added some allowed file types to this version to make it work better.

## Allow the delivery of .json, .dll, and .wasm files
Blazor requires the static file server to deliver a files that have the suffixes .json, .dll, and .wasm. By default, delivery of these types of files are not allowed. In the `AppHost.cs` file, these line instructs SS to allow the delivery of these kinds of suffixes.
```C#
this.Config.AllowFileExtensions.Add("wasm");
this.Config.AllowFileExtensions.Add("dll");
this.Config.AllowFileExtensions.Add("json");
```
## Change the default redirect path
Blazor routing requires that when the static file server sees a request made to a URL that does not match a known route, that the server return the contents of index.html. In the `AppHost.cs` file, this line instructs SS to do that.
```C#
this.Config.DefaultRedirectPath = "/index.html";
```

## Map a virtual path to the location of the files to serve
Being able to tell the ConsoleApp where the static files are located is a key requirement.You could use an absolute location, but that would not be very portable. Using a path relative to the location of the executing assembly is more portable. But how to specify that? The answer typically depends on the lifecycle stage of the application. In production, staging, and QA stages, there will be an 'AsInstalled' architecture, and the relationship of the static files to the production .exe will be known. In development under VS, the relationship of the location of static files to the location of the exe being developed, is a bit complicated.

A more sophisticated example will use SS AppSettings to create a Configuration setting value that can be controlled by a settings file. But this example will simply use a string constant. In the `AppHost.cs` file, this line, the var `physicalRootPath`, specifies the relative location of the ConsoleApp's .exe file to the Blazor app's static files.
```C#
var physicalRootPath = "../../../../GUI/bin/Debug/netstandard2.0/Publish/GUI/dist";
```
### Details on the physicalRootPath value.
The value of `physicalRootPath` shown here is specific to the way VS uses MSBuild, and to the way the GUI's Publish action uses the `DebugFolderProfile`. 
#### GUI project Publish action
Under the GUI project's Properties subfolder is the file `DebugFolderProfile.pubxml`. This file has the property \<publishUrl> which controls the location to which the GUI project is published. The Publish action creates a subfolder path *\<ProjectName>\dist* under the location specified in the \<publishUrl>. I wanted it to be easy to delete the published GUI files during development, so I decided to put the Publish location underneath the GUI project's \<OutputDir>. Since the \<publishUrl> is relative to the \<ProjectDir>, I hardcoded the path I wanted (\<OutputDir> for Debug and netstandard2.0), making it *bin\Debug\netstandard2.0\Publish*. The complete path of the published GUI will be *bin\Debug\netstandard2.0\Publish\GUI\dist*. After you have done a Publish of the GUI project, use a file explorer to verify the contents of the *\<GUIProjectDir>\bin\Debug\netstandard2.0\Publish\GUI\dist*. You will find the static files that make up the Blazor app, including the wwwroot static content and the multitude of DLLs.
#### ConsoleApp's .EXE's location after build
By default VS puts the compile/link artifacts of the ConsoleApp under it's project's MSBuild \<OutputDir>, which defaults to the *./bin/\<Config>/\<framework>* subfolder relative to the \<ProjectDir>.
Under VS debugging of the ConsoleApp, VS starts the .exe in that same \<OutputDir>.
#### Relative location of the GUI's static content to ConsoleApp's .exe location
So to map from the ConsoleApp's .exe startup directory to the GUI's static content files, the physicalRootPath value consist of:
* The four *../../.././* patterns maps to the MSBuild \<SolutionDir> top-level folder, just above each \<ProjectDir>. The \<SolutionDir> is the root folder common to both the ConsoleApp project's subfolder tree and the GUI project's subfolder tree.
* From that common folder, the GUI app's static files are down the path \<ProjectDir> (GUI), then the location specified in the DebugFolderProfile.pubxml, (*bin/\<Config>/\<Framework>/Publish*) and then */GUI/dist* as created by the Publish operation.
* Putting it all together, the ConsoleApp knows where the GUI app's static files are by going up the ConsoleApp project's directory tree to the \<SolutionDir>, then down the path to the GUI project's $\<ProjectDir> and then down into the *bin/\<Config>/\<Framework>/Publish/* specified by \<publishUrl> in `DebugFolderProfile.pubxml`, then down to the subfolder *GUI/dist* created by the Publish operation.
* Again to reiterate, the solution above is very specific to the way VS and MSBuild works, and that specific convoluted physicalRootPath is specific to the way the example project structure was organized. In production or other scenarios, the var physicalRootPath would have a different value..
  
### Details on the virtualRootPath
In the first example, the GUI uses an empty virtual path root. In the `AppHost.cs` file, this line, the var `virtualRootPath`, specifies the virtual path to the GUI Blazor app's static files. Using an empty path for the virtual path means, for this example, that Index.html is found at (http://localhost:21200/Index.html).
```C#
var virtualRootPath = "";
``` 
Later demos (hopefully) will show that non-empty values will let SS support multiple Blazor GUIs side-by-side, by aligning different virtualRootPath values with different physicalRootPath values, and modifying each Blazor GUI project's base URL routing slightly.

## Map it
In the ConsoleApp's AppHost.cs Configure method, the following line tells SS to add a new location from which to serve static files that do not match a known SS route.
```C#
this.AddVirtualFileSources.Add(new FileSystemMapping(virtualRootPath, physicalRootPath));
```
It appears wrapped in a try-catch block, to catch an exception if the physicalRootPath does not exists.

## Add CORS support to SS
Blazor apps require that Cross Browser Scripting Requests be allowed. SS makes it very easy to support CORS, by including the following lines in the ConsoleApp's AppHost.cs Configure method:
```C#
Plugins.Add(new CorsFeature(
         allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
         allowedOrigins: "*",
         allowCredentials: true,
         allowedHeaders: "content-type, Authorization, Accept"));
```
This is all that's required for SS to serve a Blazor application!

<hr>

# The ServiceStack ConsoleApp program
## The REST endpoints
SS provides the infrastructure to handle REST endpoints as well as serve the static files. Both are supported in the same SS application. Example 1 has two endpoints, whose Routes are; */Initialization* and */PostData*. Each Route has two Data Transfer Objects (DTOs), one DTO for the route's Request and one for the route's Response. Each endpoint is handled by a SS service.
## The SS Services that handle the endpoints
SS places the code that responds to a Request, and creates the Response, in methods that are part of a class that inherits from SS's Service class. There is a ton of documentation on the web about SS, and its (very feature rich) Service class. Example 1 uses just the most basic of these features. The actions that the Service takes for each endpoint are defined in the AppHost.cs file, in a class there called `BaseServices` and the two methods  therein. One method signature indicates the method should be called for a POST to the */Initialization* route, the other method's signature indicates it should be called for a POST on the */PostData* route. These methods require the DTO classes for their respective request and response. 
## TargetFramework
For Example 1, the TargetFramework for the ConsoleApp is the full .Net, Version 4.7.1 in this case.

<hr> 

# The DTOs project
Example 1 (and SS-served Blazor apps in general) will use a separate project to create a separate assembly that holds just the definitions of the DTOs. This project is referenced by both the Blazor GUI project and the ConsoleHost project. It ensure that both projects have the same definition of the data being transferred between them.

The DTO project has just one .cs file in it, CommonDTOs.cs, with all of the DTO class definition in that file.

## DTOs for Initialization Route
Both the request and response DTOs for */Initialization* are empty classes. There is no data transferred in the */Initialization* request or in its response, making this the simplest kind of Request/Response pair.
## DTOs for PostData Route
Both the request and response DTOs for */PostData* have a single Property, of type `string`, which I've chosen to call `StringDataObject`. Both the request and the response will carry a payload consisting of just this one value.
## TargetFrameworks
For Example 1, the CommonDTOs assembly has to link to both the ConsoleApp .exe and with the Blazor GUI assemblies. So the CommonDTOs project specifies a \<TargetFrameworks> of both net471 and netstandard2.0. Note the plural form of \<TargetFrameworks> used here. This produces two copies of the assembly. The other two projects reference the CommonDTOs project, and each picks up their corresponding framework-specific assembly from this project's framework-specific \<OutputDir>.

<hr>

# GUI Blazor app
The GUI app has two pages and a Nav component to move between them. It is very closely based on the "first Blazor app" example produced by the Blazor team. This example is explained here:(https://blazor.net/docs/tutorials/build-your-first-blazor-app.html)
## Index.cshtml
This is the home page of the app, and simply has some welcome text.
## BasicRESTServices.cshtml
This is the presentation page of the app that demonstrates calling into the ConsoleHost's two routes. When the page is loaded, it calls the */Initialization* route. For the */PostData* route, enter some string into the top input field, and press the submit button. It will be POSTed to the ConsoleApp, which will copy the payload from the request and put it into the response, and return it to the GUI app, where it will be displayed in the bottom field.
## BasicRESTServices.cshtml.cs
This is the codebehind page of the app that supplies the C# code referenced by the BasicRESTServices.cshtml presentation page.
## TargetFramework
Like all Blazor client-side apps, the TargetFramework for the GUI app is .Net Standard 2.0 (currently).
# Conclusion
If you are interested in using Blazor in architecture solutions that don't allow for a web server, I hope these demos help you understand one such approach that uses ServiceStack instead of a web server.

if you find errors in the code or this documentation please create a issue in the GitHub repository.

Enjoy!

<hr>

# Extras
## Starting the Monitoring tools
1. Start Fiddler, ensure it is listening to all processes. There will be a lot of cruft in the window, hundreds of request/response pairs from all the browser windows you probably have open on your development computer. It takes a while working with Fiddler to setup filters that eliminate all the other HTTP traffic coming and going in your computer, until you can see just the Blazor and ServiceStack traffic.
2. Start Sentinel, and go through its startup screens to setup the UDP listener, which will be listening for logging messages broadcast to its default listening port. This would also be a good time to inspect the NLog.config file in the example. You will see that it sends all messages from any class to two loggers, the Console logger (for the ConsoleApp's console window), and to the UDP logger as well. So for this example, the Sentinel logging program is not 100% necessary, but it will be necessary later, when ServiceStack is running in a mode that has no console (called headless mode). Getting it setup and running also makes development much easier, as the log message don't all disappear as soon as the program stops.

## launchSettings.json 
If you would like to save some keystrokes, VS can be configured to start your browser and navigate to a URL when your press F5. This is controlled by the launchSettings.json file. In this example, the launchSettings.json file is found under the Properties subfolder of the ConsoleApp's subfolder. Another launchSettings.json file is found under the Properties subfolder of the GU's subfolder. Settings `launchBrowser` to `true` and `launchUrl` to http://localhost:21200 should make this happen. (TBD, this is documented in Microsoft as working for .Net Core Web applications, and it works for one of my non-Core SS Blazor apps (ACE), but I've not yet isolated the settings needed to make it work for these Blazor demos. As of now, Publishing the GUI application causes a new browser tab to appear, but starting the ConsoleApp does not.)
 

