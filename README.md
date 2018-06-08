# BlazorExamples
Blazor-specific examples extracted and simplified from my other repositories

The idea is to have agent that runs on Windows, Linux, or mobile OS, written in C# and built against the dotnetcore target framework. The agent supports a PlugIn architecture to allow for feature expansion, and the assemblies associated with the PlugIns are built against the netstandard framework. The agent basically simply supplies, RESTful APIs on a listening port.
The human interface/deisplay of the data supplied by the Agent is done with a Blazor application. A Blazor application is written in C#, against the netstandard framework, and builds to a set of static files. Any process that can serve static files,and can perform a redirect if an unknown URL comes in on the listening port, can deliver the files necessary to run the GUI to any browser, and provide the necessary support for the blazor router. Any web-assembly compliant browser can run the GUI, on any of the Windows, Linux, or Mobile OS.
One of the Agnet plugins is designed to support the delivery of the static files needed for the GUI app.

This example is derived and simplified from the full Ace repository. AceAgent in its fullblown form is intended to be a node of a peer-to-peer distrubted network.
So when an agent is deployed to a device without the GUI feature plugin, it can (with the appropriate plugins) act as a node of a peer-to-peer distrubted network. When service is deployed with the the GUI plugin loaded, the browser on the device provide a (hopefully rich) interactive GUI to control the agent (the node), and interact with the full network of nodes.

## Architecture
An AceAgent is a general term for either an AceService or an AceDaemon. Both should be identical in APIs, although at this time there is differences in how these are handeled by their respective operationg systems, Windows or Linux.
The AceService is a Windows Service that uses the ServiceStack framework. It supplies a few basic services, and the ability to load PlugIns.
The Plugins do all the important work in the ACeAgent. Developers should be free to use any NetStandard based package or library when writing PlugIns. At the time of this writing, this was further restricted to just those netStandard libraries that will run in WebAssembly under Mono..
The Blazor GUI provides a means for humans to interact with the AceAgent. The Blazor App is built like any Blazor app, and is published to a location in the Filesystem. The GUIServices Plugin configure the AceAgent to properly serve the static files needed to load and route the Blazor app on any browser.

## General Terminology
The samples use pretty standard HTTP terminoy to talk about Request and Response pairs, URLs, ports, Verbs, and routes. Routes are a bit special in that they have the specific meaning and syntax that ServiceStack defined.
Serialization is the process of turning data structures (objects) on ether side of the network connection into a ordered serial sequence of bytes (the payload) for transmission across the network, and deserialization is the revese, it is the process of turning the payload back into an oject that code in the libraries can process.
Persistence is sometimes anothe use for serialization.Adding persistence to these examples is oemthing to be takeled later.
### Conventions
Projects and assemblies with the word Plugin in their names are part of the aceService PlugIn system.
Projects and assemblies with the word Model in their names define the APIs provided by the AceService. These assemblies provide the structure of the objects that the AceService uses for Request and Response data. These assemblies also associate route attributes with Request objects, and define which Verbs are accepted by each route. These assemblies use attributes to associate HTTP URLs (routes) with data structures.
Projects and assemblies with the word Interfaces in their names do the implementation of the logic provided by each API. They implement functions that are called by the ServiceStack framework when data is received on a route, that create the object that should be returned, and that hand the Response object back to the ServiceStack framework. error handling logic is implememnted here.  


## Projects/Assemblies
### Ace.AceGUI
a Blazor application, the example here is loaded from the specified physical path. It communicates with the Windows service using REST APIs that the service exposes.
###Ace.AceAgent (one of Ace.AceService or Ace.AceDaemon)
This example only has the Windows version, called AceService.
This assembly contains the main entry point into the agent. It is written using the ServiceStack framework (https://servicestack.net/). The ServiceStack framework is wrapped in a TopShelf wrapper (https://github.com/Topshelf) to simplify the process of installing the app as a Windows service. When run under Debug mode, as is usually the case when interactivly debugging or exploring the code, the app runs as a Console app. When run in Release mode, it expects to installed as and be running as a Windows service.
It configures ServiceStack behaviour:
-It instructs ServiceStack to generate CORS headers.
-It instructs ServiceStack to provide support for Postman.
-It instructs ServiceStack to disable support for its default metadata route.
 It supports a configuration setting file. It includes configuration settings for:
--setting the port it listens on.
It provides data structures used by the core services, and injects them into the ServiceStack Hosts' IoC container (Funq) to achieve Dependency Injection (DI) for the data objects used by the core services.
####Ace.AceService.BaseServices.Models
This assembly defines the most basic APIS in the app, those that are part of the core assembly, when no plugins are loaded. It includes the routes and classes that the AceService uses for it most basic Request and Response data structures.
#####Routes
    `[Route("/isAlive")]`
    `[Route("/isAlive/{Name}")]`

####Ace.AceService.BaseServices.Interfaces
This assembly defines the logic for hanadling the core API routes.
#####Logic
    `[Route("/isAlive")]`
    `[Route("/isAlive/{Name}")]`
    Return the string "Hello" for the route [Route("/isAlive")], and returns the string "Hello " concatenated with the Name field of this route's Request object.

####Ace.AceService.GUIServices.Models
This assembly defines an API that supports the GUIs configured/supported by this PlugIn It includes the routes and classes that the GUIServices PlugIn uses for it's Request and Response data structures.
#####Routes
  `[Route("/VerifyGUI")]`
  `[Route("/VerifyGUI/{Kind};{Version}")]`

####Ace.AceService.GUIServices.Interfaces
This assembly defines the logic for handling the GUIServices API routes.
#####Logic
  `[Route("/VerifyGUI")]`
  `[Route("/VerifyGUI/{Kind};{Version}")]`
    Return the string "Blazor" for the route [Route("/VerifyGUI")], and returns the string "true" or "false" for the route [Route("/VerifyGUI"/{Kind};{Version})], depending on if Kind matches the string "Blazor" and Version matches the string "0.3.0". These are currently just hardcoded in this example.

####Ace.AceService.GUIServices.Plugin
This assembly provides the entry point into the GUIServices PlugIn. It provides data structures used by the PlugIn, and injects them into the ServiceStack Hosts' IoC container (Funq) to achieve Dependency Injection (DI) for the data objects used by the plugin.
#####Logic 
It supports a configuration setting file. It includes configuration settings for:
-setting the root of the virtualpath to either "" or some other string (should be an string that is acceeptable as a URL virtual path root).
-specifying a path on the computer's filesystem from which to serve static files.
It configures ServiceStack behaviour:
-It instructs ServiceStack to respond to requests on the virtual path with a file name if the request's URL matches a file name found in the virtualpath. It follows subdirs as well.
-It instructs ServiceStack to respond to requests on the virtual path with a redirect to virtualpath/index.html, if the request on the virtual path does not match any file name.
