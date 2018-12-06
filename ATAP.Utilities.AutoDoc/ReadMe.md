# Autodoc for the GitHub BillHertzing BlazorExaples Repository

AutoDoc is an experimental project to crete a static website for documentation of a repository. The project is part of the ATAP.Utilities repository, here: 

Please refer to the AutoDoc documentation for detailed explanation of how the project creates the static documentation website (DocWebSite).

In this implementation
    * the following are created manually.
        * repository-level ReadMe.md This is the same ReadMe.md that GitHub displays for the repository.
        * repository-level index.md This is the repository-level landing page for the DocWebSite. It will be converted to index.html at the root of the DocWebSite.
		* repository-level toc.yml (or toc.md). This is the repository level Table Of Contents (toc). Usually, a simple repository will have Home, Articles, and API sections.
		     * The Home link in the toc points to the index.html file in the root of the DocWebSite.
		     * The Articles link in the toc points to the index.html file in the Articles subfolder one level below the root of the DocWebSite.
			 * The API link in the toc points to the index.html file in the API subfolder one level below the root of the DocWebSite.
        * repository-level Docs subfolder and its contents/ This is the place for QuickStarts, tutorials, documents on building and running the code in the repository, etc. Whatever additional documentaion you crete for the repository as a whole goes in here. Subfolders under Docs are not yet supported.
		     * Under the repository-level Docs subfolder you will need
			 * index.md. This is the landing page for the Articles subfolder under the root of the DocWebSite. It will be converted to index.html at the root of the Articles subfolder.
			 * toc.yml (or toc.md). This is the Articles subfolder level Table Of Contents. It will be converted to toc.html at the root of the Articles subfolder, and will be included as the contents in the left-side navigation pane For all Articles pages.
        * project-level ReadMe.md This is the same ReadMe.md that GitHub displays for the each project in the repository.
		* project-level index.md This is the project-level landing page for each projects' documentation subfolder under the Articles subfolder. It will be converted to index.html at the root of the subfolder created for each project under Articles.
	* The following images are added to the AutoDoc project to provide image resources
	    * ataplogo1inch.bmp - this is the image used for the logo in the NavBar. ToDo is to replace this with a SVG file someday...

# Template modification
## location
Template modification are found here: `BlazorExamples\ATAP.Utilities.AutoDoc\templates\`. There are two subfolders, `AutoDocTemplate01` and `AutoDocTemplate02`, that provide the normal release template, and a second one that I can use to play around with new ideas without changing the normal release template.
## partials modifications
### logo.tmpl.partial
Modifies the default logo template provided with DocFx, it changes the Alt Text for the logo. Someday it would be nice to find the time to make a feature request to DocFx team that this should be made modifiable through the GlobalMetadata
This project modifies the default footer provided with DoxFx. 
### footer.tmpl.partial
Modifies the default footer seen on every page provided with DocFx, it changes the entire footer text, and adds a contributions popup.
## StyleSheet Modifications
Modifies the default CSS provided with DocFx, this project does not yet modify any of the default CSS that comes with DocFx, but each template has an empty placeholder to allow for future customization in this area.

#  DocFx.Json 
The file docfx.json is key to the creation of the website.
## globalMetadata
The following globalMeta settings are different from the defaults provided by DocFx
    * _appTitle is the title tag of every page (?). For this repository, it is "Blazor Examples with ServiceStack".
	* _appLogoPath is the path, realtive to the root of the DocWebSite, where the logo for the NavBar will be found. By default, the `logo.tmpl.partial` provided by DocFx expects a .svg file. I've found that simply using a .bmp file here will work. Issue (tbd) is to gain a better understanding of how DocFx uses both Liquid templates and partial templates, and 'do the right thing" here."'
	* _disableContribution: "true" DocFx allows for public (?) or GitHub users with Contributor's role (?) to submit a change to the documentation directly to a branch on Gitub. This feature is turned off until I have a better understanding of what happens if alllowed..
## Create a searchable Index
`"postProcessors": [ "ExtractSearchIndex" ],`
