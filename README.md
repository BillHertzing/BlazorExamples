# Examples of Blazor Apps served By ServiceStack

Blazor is an experimental technology fgrom Microsoft that allows applications written in C# to run on any browser that supports WASM. Many other folks have written better introductions and explanations of Blazor and WASM than I can, so please search the Web for those terms if you would like detailed background information on this emerging technology.

Blazor applications can run server-side (Server-Side Blazor, or SSB), or client-side (Client-Side Blazor, or CSB). The examples in this repository are all about client-side Blazor. The files needed to run a Blazor application can be served to a browser by any process that understands HTTP and can serve static files. Of course, all the popular web server packages can do this, as can many Cloud services. There are many resources on the web that can go into much greater detail for using those technologies.

However, I've been looking for a way to leverage the browser as a GUI for an application that runs on multiple operating systems. Most application that need a GUI have to create the GUI specifically for an OS. Blazor brings the ability for developers to write their GUI in C#, publish it to a set of static files, and have any any browser render and run the GUI. An application that leverages .Net, .Net Standard, and .Net Core to run on multipleOS, combined with a Blazor-based GUI, promises to greatly reduce the platform-specific portions of any multi-OS application.

ServiceStack is a very popular product that provides REST endpoints for an application. ServiceStack can also serve static files. Combining the two, ServiceStack can  serve the files needed for a Blazor GUI, and can also serve the REST endpoints that allow the GUI to communicate with the application. 

This repository will focus specifically on using the ServiceStack application to serve and interact with a Blazor application. The contents of this repository will be demonstration programs showing how to integrate Blazor with various ServiceStack features. These examples are simplified versions of the ACE application and it's Blazor GUI, which is in the Ace repository adjacent to this one.

The first demo program is the most basic. The Blazor GUI portion consists of two Razor pages, and code that makes two REST calls to ServiceStack, one REST call with no data sent, and one that sends a string and receives a string. The ServiceStack application portion consist of a Console program for Windows which serves the static files for the Blazor application, and handles the two simple REST service endpoints. 

All of the following instructions assume you are using a Visual Studio (VS) 2017 IDE for development, and are pretty familiar with using GIT and GitHub in VS.

Prerequisites:
Visual Studio 2017 Vx.xx or newer
Blazor 0.6.0 components installed into VS, See instructions here.  Blazor is changing rapidly, and I will do my best to ensure that the examples in this repository track the changes in Blazor.
Blazor logging framework installed into VS here
ServiceStack (SS) free edition Version 5.4.1 installed into VS. See instructions here. SS free edition  has a limitation of 10 services. Each of the demonstration programs here will be written to stay below the limit.
A logging framework such as Open Source Software NLog installed into VS. The examples here use  NLog. NLog's configuration file for tehse examples also specifies a UDP-based logger.
The free UDP logging application Sentinel V0.13.0.0, which can be installed to Windows from here.
Telerik's  free Fiddler 4 product or equivalent for monitoring the HTTP traffic between the browser and teh ServiceStack instance, whihc can be instgalled from here

You should alo be aware that ServiceStack development team does a great job of patching and enhancing ServiceStack, and there my be times you will want to get new patches from ServiceStack's MyGet feed. Instructions (here)[https://www.myget.org/F/servicestack].

Getting Started:
Install the prerequisites listed above onto your development computer
Create a local copy (fork) of this repository

STOPPING point
The AceAgent basically supplies RESTful APIs on a listening port.

The human interface/display of the data supplied by the AceAgent is done with a Blazor application, called AceGUI. The AceGUI, like any Blazor application is written in C#, targets the .Net Standard framework, and builds to a set of static files. 

Any process that can serve static files,and can perform a redirect if an unknown URL comes in on the listening port, can deliver the files necessary to run the GUI to any browser, and provide the necessary support for the Blazor router. Any browser that supports WebAssembly can run the AceAgent Blazor GUI. One of the AceAgent PlugIns is designed to provide the necessary support needed to run the AceAgent Blazor GUI.

Taken all together, the AceAgent, AceGUI, and other PlugIns should get closer to the "write once, run everywhere" nirvana that software developers have striven for since the early 1980s 

This example is derived from and the full Ace repository. AceAgent in its full-blown form is eventually intended to be a node of a peer-to-peer distributed network that provides a large number of features.

AceAgent should be capable of deploying to extremely tiny memory/process space footprint, and scale up its footprint as features/PlugIns are added. An AceAgent can be deployed to a device without the GUI feature PlugIn if the footprint must be kept minimal, or if the device does not have a need to provide a GUI. When AceAgent is deployed with the the Blazor GUI PlugIn loaded, the browser on the device can provide a (hopefully rich) interactive GUI to control the AceAgent node, and interact with the full network of nodes.

## Architecture
An AceAgent is a general term for either an AceService or an AceDaemon. Both should be identical in APIs, although at this time there are differences in how these are handled by their respective operating systems, Windows or Linux.

The AceService is a Windows Service that uses the ServiceStack framework. It supplies a few basic services, and the ability to load PlugIns.

The PlugIns do all the important work in the AceAgent. Developers should be free to use any .Net Standard based package or library when writing PlugIns. At the time of this writing, this was further restricted to just those .Net Standard libraries that will run in WebAssembly under Mono.

The AceAgent Blazor GUI provides a means for humans to interact with the AceAgent. The AceAgent Blazor GUI is built like any Blazor app, and is published to a location in the File system. The GUIServices PlugIn configures the AceAgent to properly serve the static files needed to load and route the Blazor app on any browser.

## General Terminology
Throughout the documentation AceAgent will be used to refer to the application whenever the context is that of the general application. AceService and AceDaemon will be used when the context is that of the specific version of AceAgent running on a specific OS. Note that the project/assembly names currently are named AceService - this will change in a future release to AceAgent.

The samples use pretty standard HTTP terminology to talk about Request and Response pairs, URLs, ports, Verbs, and routes.

Routes are a bit special in that they have the specific meaning and syntax that ServiceStack defined.

Serialization is the process of turning data structures (objects) on ether side of the network connection into a ordered serial sequence of bytes (the payload) for transmission across the network.

De-serialization is the reverse of Serialization, it is the process of turning the payload back into an object that code in the libraries can process.

Persistence is sometimes another use for serialization. A future enhancement to this project will add persistence to the AceAgent.

### Conventions
DTOs 


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
