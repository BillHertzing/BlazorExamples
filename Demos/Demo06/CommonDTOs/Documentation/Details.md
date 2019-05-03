# Common Data Transfer Objects (DTOs) Project/Assembly
All Demos (and SS-served Blazor apps in general) uses a separate project to create a separate assembly that holds just the definitions of the DTOs. This project is referenced by both the Blazor GUI project and the ConsoleHost project. It ensure that both projects have the same definition of the data being transferred between them.

The CommonDTOs project has just one .cs file in it, CommonDTOs.cs, with all of the DTO class definition in that file.

## DTOs for the */Initialization* Route
Both the request and response DTOs for */Initialization* are empty classes. There is no data transferred in the */Initialization* request or in its response, making this the simplest kind of request/response pair.
## DTOs for the */PostData* Route
Both the request and response DTOs for */PostData* have a single property, of type `string`, which I've chosen to call `StringDataObject`. Both the request and the response will carry a payload consisting of just this one value.
## TargetFrameworks
For all Demos, the CommonDTOs assembly will need to link to both the `ConsoleApp.exe` and with the Blazor GUI assemblies. So the CommonDTOs project specifies a `<TargetFrameworks>` of both net471 and netstandard2.0. Note the plural form of `<TargetFrameworks>` used here. This produces two copies of the assembly. The other two projects each reference the CommonDTOs project, and each picks up their corresponding framework-specific assembly from this project's framework-specific `<OutputDir>`.
