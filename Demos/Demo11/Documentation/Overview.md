# Overview of Demo11
The purpose of Demo11 is to show the simplest windows service running a Generic Host hosting the Kestrel webserver

## Server 
Added to the generic host from the previous demo, is a class that implements the ServiceLifetime for the GenericHost. Attribution
The Server program creates an instance of a Generic Host that hosts the Kestrel web serve that itself hosts the ServiceStack middleware instantiaed from  SS's `AppHostBase`. The Server program initailizes the generic host, the kestrel web server and the SS middleware,  initializes it, and starts it listening. The Server responds on two specific URLs (the routes), and responds with the contents of index.html when '/'.  When the Blzor GUI gets back a response containingg index.html's contents, an request comes for index.html. the page instructs the browser to make many more calls to the ConsoleApp, to fetch the .DLL files, and resource files, needed to render the GUI.

## The Blazor GUI
The GUI is comprised of a home page and a second page with an input field, a button, and a place to show the results of an API call. The home page is index.html, and contains instructions on what to download from the Server.

## The CommonDTOs
The CommonDTOs define objects that contain the data fields that will be transmitted between the GUI and the Server.

