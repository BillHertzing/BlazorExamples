# The ServiceStack (SS) ConsoleApp.exe Project / Assembly / executable program
A simple Console program the implements the ServiceStack console host for two endpoints, plus the sauce needed to make ServiceStack deliver the static files that make up the GUI.

# The REST endpoints
SS provides the infrastructure to handle REST endpoints as well as serve the static files. Both are supported in the same SS application. Demo01 has two endpoints, whose Routes are; */Initialization* and */PostData*. Each Route has two Data Transfer Objects (DTOs), one DTO for the route's Request and one for the route's Response. Each endpoint is handled by a SS service. 
# The SS Services that handle the endpoints
SS places the code that responds to a Request, and creates the Response, in methods that are part of a class that inherits from SS's Service class. There is a ton of documentation on the web about SS, and its (very feature rich) Service class. Demo01 uses just the most basic of these features. In Demo01, the actions that the Service takes for each endpoint are defined in the AppHost.cs file, in a class there called `BaseServices` and the two methods  therein. One method signature indicates the method should be called for a POST to the */Initialization* Route, the other method's signature indicates it should be called for a POST on the */PostData* Route. These methods signatures specify the DTO classes for their respective Request and Response. 
# TargetFramework
For Demo01, the TargetFramework for the ConsoleApp.exe program is the full .Net, Version 4.7.1 in this case.

# How to make ServiceStack deliver the Blazor app
You will need to start with version  5.4.1 or higher, because ServiceStack developers added some allowed file types to this version to make it work better. The following discussion applies to the code found in `AppHost.cs`.

## Allow the delivery of .json files
Blazor requires the static file server to deliver a file named `blazor.boot.json` from the `_frameworks` subfolder. By default, delivery of .json files are not allowed. In the `AppHost.cs` file, this line instructs SS to allow the .json suffix.
```C#
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
Later examples (hopefully) will show that non-empty values will let SS support multiple Blazor GUIs side-by-side, by aligning different virtualRootPath values with different physicalRootPath values, and modifying each Blazor GUI project's base URL routing slightly.

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

