# BlazorExamples
Blazor-specific examples extracted and simplified from my other repositories

The goal is to have program, called AceAgent, that runs on Windows, Linux, and mobile OSs. AceAgent should:
* Be written in C# and targets the .Net Core framework, and provide a very minimal set of basic services.
* Support a PlugIn architecture to allow for user-selectable expansion of the services provided.
* The assembly that represents the main entry point should be built against an OS-specific framework. For this example project, only the Windows architecture is shown, and the main entry point assembly targets .Net 4.7
* All other assemblies should target the .Net Standard framework, and be compatible across all OSs.

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
Projects and assemblies with the word _PlugIn_ in their names are part of the aceService PlugIn system.

Projects and assemblies with the word _Model_ in their names define the APIs provided by the AceService. These assemblies provide the structure of the objects that the AceService uses for Request and Response data. These assemblies also associate route attributes with Request objects, and define which Verbs are accepted by each route. These assemblies use attributes to associate HTTP URLs (routes) with data structures. They are also the place where route attributes and guards built in to ServiceStack can be applied.

Projects and assemblies with the word _Interfaces_ in their names do the implementation of the logic provided by each API. They implement the functions that are called by the ServiceStack framework when data is received on a route, that create the Response object that should be returned, and that hand the Response object back to the ServiceStack framework. Error handling logic is implemented here.  


## Projects/Assemblies
For details on the Blazor GUI application, look at the project Ace.AceGUI. For details onthe AceAgent applicationthat serves the aceGUI static files and provides API endpoints, see thesection Ace.AceAgent.
### Ace.AceGUI
Most folks who come here for the Blazor examples will be interested primarly in this project/assembly. This Blazor GUI App is derived from the stock examples found on GitHub.
###Pages
####BaseServices
Display a simple page that interfaces with the APIs provided in the core services of the AceAgent.
####GUIServices
Display a simple page that interfaces with the APIs provided in the GUIServices PlugIn.
####ComputerInventory.Hardware
Display a simple page that contains the first Blazor components being developed in this repository.
### Components
####DropDownSingleSelectOfEnum
This component will accept an enumeration type, an initial enumeration value, and display a list of enumeration's values focused on the initial value. The string displayed in the list should be:
* the value of a custom attribute on that enum value (currently hardcoded as [SpecialDescription])
* the value of the [Description] attribute on that enum value
* the Name of the enum value
The component maintains it currentValue in a private field, and provides access to this value through the Currentvalue property.
When another piece of code changes the value of CurrentValue, the setter mthod calls StateHasChanged() to tell Blazor that the component needs to be re-rendered.

This component is interesting becasue it should be able to be declared to use any enumeration type, and the initialValue, currentValue, and Currentvalues should all accept/return enumerations of whatever type is used to instantiate the component.

In the next iteration of the component, I hope to be able to allow a user to specify an ordered list of custom attributes to display, instead of the current hardcoded three.
In the next iteration of the component, I hope to be able to provide an OnChanged event, and trigger this event if CurrentValaue iis changed (in the setter), or, if the user changes the currrentValue by changing the selected element of teh dropdownlist.

### Ace.AceAgent (one of Ace.AceService or Ace.AceDaemon)
This example only has the Windows version, called AceService. 
This assembly contains the main entry point into the agent. It is written using the ServiceStack framework (https://servicestack.net/). The ServiceStack framework is wrapped in a TopShelf wrapper (https://github.com/Topshelf) to simplify the process of installing the AceService as a Windows service. When run under Debug mode, as is usually the case when interactively debugging or exploring the code, the AceAgent runs as a Console App. When run in Release mode, it expects to installed as and be running as a Windows service.
#### Logic
It configures ServiceStack behavior as follows:

* It instructs ServiceStack to generate CORS headers.
* It instructs ServiceStack to provide support for Postman.
* It instructs ServiceStack to disable support for its default metadata route.
* It supports a configuration settings file, which currently includes configuration settings for:
  * setting the port AceAgent listens on.

It provides data structures used by the core services, and injects them into the ServiceStack Hosts' IoC container (Funq) to achieve Dependency Injection (DI) for the data objects used by the core services.
#### Ace.AceService.BaseServices.Models
This assembly defines the most basic APIs in the AceAgent, those that are the core services, when no PlugIns are loaded. It includes the routes and classes that the AceService uses for it most basic Request and Response data structures.
##### Routes
    `[Route("/isAlive")]`
    `[Route("/isAlive/{Name}")]`

#### Ace.AceService.BaseServices.Interfaces
This assembly defines the logic for handling the core API routes.
##### Logic
    `[Route("/isAlive")]`
    `[Route("/isAlive/{Name}")]`
    Return the string "Hello" for the route [Route("/isAlive")], and returns the string "Hello " concatenated with the Name field of this route's Request object.

#### Ace.AceService.GUIServices.Models
This assembly defines an API that supports the GUIs configured/supported by this PlugIn It includes the routes and classes that the GUIServices PlugIn uses for it's Request and Response data structures.
##### Routes
  `[Route("/VerifyGUI")]`
  `[Route("/VerifyGUI/{Kind};{Version}")]`

#### Ace.AceService.GUIServices.Interfaces
This assembly defines the logic for handling the GUIServices API routes.
##### Logic
  `[Route("/VerifyGUI")]`
  `[Route("/VerifyGUI/{Kind};{Version}")]`
    Return the string "Blazor" for the route [Route("/VerifyGUI")], and returns the string "true" or "false" for the route [Route("/VerifyGUI"/{Kind};{Version})], depending on if Kind matches the string "Blazor" and Version matches the string "0.3.0". These are currently just hardcoded in this example.

#### Ace.AceService.GUIServices.PlugIn
This assembly provides the entry point into the GUIServices PlugIn. It provides data structures used by the PlugIn, and injects them into the ServiceStack Hosts' IoC container (Funq) to achieve Dependency Injection (DI) for the data objects used by the PlugIn.
##### Logic 
It supports a configuration setting file. It includes configuration settings for:
-setting the root of the virtualpath to either "" or some other string (should be an string that is acceptable as a URL virtual path root).
-specifying a path on the computer's file system from which to serve static files.
It configures ServiceStack behavior:
-It instructs ServiceStack to respond to requests on the virtual path with a file name if the Request's URL matches a file name found in the virtualpath. It maps subdirectories of the physical path to subdirectories of the virtual path, and delivers files from these locations as well.
-It instructs ServiceStack to respond to requests on the virtual path with a redirect to virtualpath/index.html, if the request on the virtual path does not match any file name.
