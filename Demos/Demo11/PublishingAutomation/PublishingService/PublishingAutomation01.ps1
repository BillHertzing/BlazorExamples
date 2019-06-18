# Set-executionpolicy for Visual Studio 15 (Poshtools and Nuget) requires it be set in the 32-bit Poshtools window in addition to globally
# Windwos 10 indexing - need to set .ps1 and .pdm1 to full-text indexing

#############################################################################  
# Help Format
#region ScriptCommentBlock
#region ModuleCommentBlock
#region FunctionCommentBlock
<#
.SYNOPSIS 
Provides automation functions for a CLI to help publish a .Net Core 3.0 webHost as a Windows service
.DESCRIPTION
Help For this module.
.PARAMETER startingRootDir
Specifies ...
.PARAMETER installedToRootDir
Specifies ...
.INPUTS
Inputs to this function are Build artifacts in a directory tree rooted at `startingRootDir`
.OUTPUTS
Outputs from this function are Deploy/Install artifactsto a directory tree rooted at `installedToRootDir` for a Windows Service consisting of a genericHost hosting a Kestrel-only webHost hosting ServiceStack Middleware that serves the static assets of a Blazor GUI
.EXAMPLE
C:\PS> pa packageAndDeploy
.EXAMPLE
C:\PS> pa stop
.EXAMPLE
C:\PS> pa start
.LINK
http://www.somewhere.com/attribution.html
.LINK
<Another link>
.ATTRIBUTION
<Where the ideas came from>
.SCM
Configuration management under Git with GitFlow extensions
ToDo: <Configuration Management Keywords>
#>
#endregion FunctionCommentBlock
#endregion ModuleCommentBlock
#endregion ScriptCommentBlock
############################################################################# 


############################################################################# 
# packageAndDeploy
#region FunctionCommentBlock
<#
.SYNOPSIS 
the packageAndDeploy command copies the files needed to deploy the serice from the startingRootDir to the installedToRootDir
.DESCRIPTION

#>
#endregion FunctionCommentBlock
function packageAndDeploy {
#region FunctionParameters
[CmdletBinding(DefaultParameterSetName='Min')]
Param(
  [Parameter(Mandatory=$true, ParameterSetName='Min')]
  [Parameter(Mandatory=$true, ParameterSetName='MinLogger')]
  [Alias('StartingRootDir')]
     $InDir,
  [Parameter(Mandatory=$true, ParameterSetName='Min')]
  [Parameter(Mandatory=$true, ParameterSetName='MinLogger')]
  [Alias('InstalledToRootDir')]
     $OutDir
  [Parameter(Mandatory=false, ParameterSetName='Min')]
  [Parameter(Mandatory=false, ParameterSetName='MinLogger')]
  [Alias('F')]
    $Force,
  [Parameter(Mandatory=false, ParameterSetName='Min')]
  [Parameter(Mandatory=false, ParameterSetName='MinLogger')]
  [Alias('Name')]
    $ServiceName
)
#endregion FunctionParameters

  $settings=@{
    InDir = '.\Inputs'
    OutDir = '.\outputs'
    Force = $false
	ServiceName = 'Demo11'
	CopyRules = @{}
  }

  # Things to be initialized after settings are processed
  if ($InDir) {$Settings.InDir = $InDir}
  if ($OutDir) {$Settings.OutDir = $OutDir}
  if ($Force) {$Settings.Force = $Force}
  if ($ServiceName) {$Settings.ServiceName = $ServiceName}

  $DebugPreference = 'Continue'

  # In and out directory and file validations
  if (-not (test-path -path $settings.InDir -pathtype Container)) {throw "$settings.InDir is not a directory"}

  # Output tests
  if (-not (test-path -path $settings.OutDir -pathtype Container)) {throw "$settings.OutDir is not a directory"}
  # Validate that the $Settings.OutDir is writeable
  $testOutFn = $settings.OutDir + 'test.txt'
  try {new-item $testOutFn -force -type file >$null}
  catch {#Log('Error', "Can't write to file $testOutFn"); 
  throw "Can't write to file $testOutFn"}
  # Remove the test file
  Remove-Item $testOutFn

  ## For creation of new output files
  #$OutFnPCARules = join-path $settings.OutDir $settings.OutFnPCARules
  #$OutFnHBRRules = join-path $settings.OutDir $settings.OutFnHBRRules
  #$OutFnOnDemandRules = join-path $settings.OutDir $settings.OutFnOnDemandRules

  # Validate that all of the expected files exist in the startingRootDir and ensure each is readable
  # Copy _PublishedAgent\_PUblishedService\
  # Stop (kill -9) every process matching processnamepattern ... this could be dangerous. Catch exceptions if any Stop signal fails
  # Delete everything (files and subtrees) in the installedToDir if force is true ... this could be dangerous. Catch exceptions if any delete fails
  # copy all of the expected files and directory subtrees from the startingRootDir installedToRootDir. Catch exceptions if any copy fails.
}

############################################################################# 

############################################################################# 
# Stop
#region FunctionCommentBlock
<#
.SYNOPSIS 
the Stop command looks in the processtable, finds all processes with the configured serviceName, and stops them
.DESCRIPTION

#>
#endregion FunctionCommentBlock

#region FunctionEndBlock
########################################
END {

#endregion FunctionEndBlock
}

############################################################################# 


############################################################################# 
# Function Template

#region FunctionCommentBlock
<#
.SYNOPSIS 
This Function tests a range of ports for a listener present
.DESCRIPTION
Takes a list of computer names and a list of ports, returns a list of objects having a boolean IsConnected
#>
#endregion FunctionCommentBlock
function FunctionNAme {
#region FunctionParameters
[CmdletBinding(DefaultParameterSetName='Min')]
Param(
  [Parameter(Mandatory=$true, ParameterSetName='Min')]
  [Parameter(Mandatory=$true, ParameterSetName='MinLogger')]
  [Alias('Names', 'ComputerNames')]
    [Object[]] $Cn,
  [Parameter(Mandatory=$true, ParameterSetName='Min')]
  [Parameter(Mandatory=$true, ParameterSetName='MinLogger')]
  [Alias('PortList')]
    [Object[]] $Ports
  [Parameter(Mandatory=$true, ParameterSetName='Min')]
  [Parameter(Mandatory=$true, ParameterSetName='MinLogger')]
  [Alias('OutDir')]
    [Object] $OutDir
  [Parameter(Mandatory=$true, ParameterSetName='Min')]
  [Parameter(Mandatory=$true, ParameterSetName='MinLogger')]
  [Alias('OutFn')]
    [Object] $OutFn

)
#endregion FunctionParameters

#endregion



#region FunctionBeginBlock
########################################
BEGIN {
#region DefaultValues and command line settings
  $settings=@{
    Cn = 'localhost'
    Ports = 20000..22000;
    OutDir = '.\logs'
    OutFn = 'PortsWithListeners ' + +(Get-Date).ToString('yyyyMMdd')+'.log'
  }

    # Things to be initialized after settings are processed
  if ($Cn) {$Settings.Cn = $Cn}
  if ($Ports) {$Settings.Ports = $Ports}
  if ($OutDir) {$Settings.OutDir = $OutDir}
  if ($OutFn) {$Settings.OutFn = $OutFn}

  $DebugPreference = 'Continue'

  # Output tests
  if (-not (test-path -path $settings.OutDir -pathtype Container)) {throw "$settings.OutDir is not a directory"}
  # Validate that the $Settings.OutDir is writeable
  $testOutFn = $settings.OutDir + 'test.txt'
  try {new-item $testOutFn -force -type file >$null}
  catch {#Log('Error', "Can't write to file $testOutFn"); 
  throw "Can't write to file $testOutFn"}
  # Remove the test file
  Remove-Item $testOutFn
  $OutFn = join-path $settings.OutDir $settings.$OutFn

  @acc = {}
}
#endregion FunctionBeginBlock

#region FunctionProcessBlock
########################################
PROCESS {
	foreach ($c in $Cn) {
		foreach ($p in $Ports) {
			Test-NetConnection -	
		}
	}
}
#endregion FunctionProcessBlock

#region FunctionEndBlock
########################################
END {

}
#endregion FunctionEndBlock
}

############################################################################# 
 
############################################################################# 
# Function Template

#region FunctionCommentBlock
<#
.SYNOPSIS 
Help For this module
.DESCRIPTION
Help For this module.
#>
#endregion FunctionCommentBlock
function FunctionNAme {
#region FunctionParameters
[CmdletBinding(DefaultParameterSetName='Min')]
Param(
  [Parameter(Mandatory=$true, ParameterSetName='Min')]
  [Parameter(Mandatory=$true, ParameterSetName='MinLogger')]
  [Alias('Path')]
    [Object[]] $ScriptPath
)
#endregion FunctionParameters

#region FunctionBeginBlock
########################################
BEGIN {
}
#endregion FunctionBeginBlock

#region FunctionProcessBlock
########################################
PROCESS {
}
#endregion FunctionProcessBlock

#region FunctionEndBlock
########################################
END {
}
#endregion FunctionEndBlock
}

############################################################################# 

#############################################################################
# SET BREAKPOINT!
# http://mshforfun.blogspot.com/2006/06/enter-nested-prompt-function.html
# $Host.EnterNestedPrompt() ### Uncomment to force a breakpoint here
#############################################################################

