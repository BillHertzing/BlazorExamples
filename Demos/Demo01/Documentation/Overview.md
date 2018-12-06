# Overview of Demo01
The purpose of Demo01 is to show the near minimal code needed to deliver the Blazor GUI, and provide two simple REST endpoints that can communicate with the Blazor GUI.

## ConsoleApp 
The ConsoleApp program creates an instance of a ServiceStack `AppSelfHostBase`, initializes it, and starts it listening. The ConsoleApp responds on two specific URLs (the routes), and responds with the index.html when any unknown route is sent.When an request comes for index.html. the page instructs the browser to make many more calls to the ConsoleApp, to fetch the .DLL files, and resource files, needed to render the GUI.

## The Blazor GUI
The GUI is comprised of a home page and a second page with an input field, a button, and a place to show the results of an API call. The home page is index.html, and contains instructions on what to download from the ConsoleApp.

## The CommonDTOs
The CommonDTOs define objects that contain the data fields that will be transmitted between the GUI and the ConsoleApp.

