# Overview the Documentation
Conceptual Documentation, such as Getting Started guides, Building guides, Attribution, and Contribution Guidelines, are all stored under Articles or a subfolder of that.
The ReadMe.ms files that GitHub expects for the repository at the solution level and at each project level are all found under the ReadMes subfolder.

The solution and every project can have a index.md file. This file is the home page in the documentation site for each project and index.md at the Solution level is the documentation home page for the entire Solution. Finally the Solution contains a toc.yml file, which has three parts. Each part consists of a name, base UI, and optional homepage relative URI.Right now, the toc.yml for teh entire solution consists of a Home page, and API subfolder, and an Articles subfolder.
 
Project level:
ReadMe.md - shown on gitHub for the project's ReadMe
Docs/* files like quickstart (or getting started), building, and attribution, for each project

Solution Level
ReadMe.md - the Repository ReadMe files
Docs/* files in the Docs solution folder of the solution
Attribution.md - a conglomeration. Part of ot is links that are recorded at the Solution level in teh Docs/Attribution.md file, and a set of links defined by the Docs/attribution.md files found in each Project.
QuickStart.md - How to fork the repository, how to NuGet the libraries (released versions and patch versions), how to get and use the AutoDoc, how to get and use the BuildTooling extensions, how to get and install the PoweShellscripts, how to integrate the PowerShellscripts with VS,)