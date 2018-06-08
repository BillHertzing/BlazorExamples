param (
  [string] $TargetName,
  [string] $Version
  )
function Set-NuSpecVersion {
param (
[string]$ProjectDir,
[string]$Version
)
  write-host "Starting Set-NuSpecVersion projectDir = $ProjectDir, Version = $Version"
    
    #write new version to nuspec
    $nuspec_file_name = "Package.nuspec"

    $nuspec_file_path = "$ProjectDir$nuspec_file_name"
    Write-Host "nuspec file: $nuspec_file_path"
    [xml]$nuspec_xml = Get-Content($nuspec_file_path)
    $nuspec_xml.package.metadata.version = $Version
    $nuspec_xml.Save($nuspec_file_path)
    write-host "Ending Set-NuSpecVersion"
    }

    Function Clear-BrowserCache {
      [CmdletBinding(SupportsShouldProcess)]
      param (
        [Parameter(Mandatory=$True)]
        [ValidateSet("Chrome","Firefox")]
        [string[]] $Browser,
        [Parameter(Mandatory=$True)]
        [ValidateSet("Archived History", "Cache", "Cookies", "History", "Login Data", "Top Sites", "Visited Links", "Web Data")]
        [string[]] $ItemsToDelete,
        [string] $Name = $env:USERNAME,
        [int] $DaysToDelete = 1
        )
  write-host "Starting Clear-BrowserCache Browser = $Browser, Name = $Name, DaysToDelete = $DaysToDelete"
      # $Items = @("*Archived History*", "*Cache*", "*Cookies*", "*History*", "*Login Data*", "*Top Sites*", "*Visited Links*", "*Web Data*")
      # Get-ChildItem $crLauncherDir, $chromeDir, $chromeSetDir -Recurse -Force -ErrorAction SilentlyContinue | 
      #    Where-Object { ($_.CreationTime -lt $(Get-Date).AddDays(-$DaysToDelete)) -and $_ -like $item} | ForEach-Object -Process { write-host "Remove-Item $_ -force -Verbose -recurse -ErrorAction  SilentlyContinue" }
      $pathsTosearchForItemsToDelete = @()
      $GoogleCacheRelativePath='Google\Chrome\User Data\Default\Cache'
      $FirefoxCacheRelativePath='Mozilla\Firefox\Profiles\*.default\cache*'
      # ToDo expand to include other kinds of browser items to delete
     # $temporaryIEDir = "C:\users\*\AppData\Local\Microsoft\Windows\Temporary Internet Files\*" ## Remove all files and folders in user's Temporary Internet Files. 
     # $cachesDir = "C:\Users\*\AppData\Local\Microsoft\Windows\Caches"  ## Remove all IE caches. 
     # $cookiesDir = "C:\Documents and Settings\*\Cookies\*" ## Delets all cookies. 
     # $locSetDir = "C:\Documents and Settings\*\Local Settings\Temp\*"  ## Delets all local settings temp 
     # $locSetIEDir = "C:\Documents and Settings\*\Local Settings\Temporary Internet Files\*"   ## Delets all local settings IE temp 
     # $locSetHisDir = "C:\Documents and Settings\*\Local Settings\History\*"  ## Delets all local settings history
     # $crLauncherDir = "C:\Documents and Settings\%USERNAME%\Local Settings\Application Data\Chromium\User Data\Default"
     # $chromeDir = "C:\Users\*\AppData\Local\Google\Chrome\User Data\Default"
     # $chromeSetDir = "C:\Users\*\Local Settings\Application Data\Google\Chrome\User Data\Default"
     # C:\Users\$($_.Name)\AppData\Local\Mozilla\Firefox\Profiles\*.default\cache\* -Recurse -Force -EA SilentlyContinue -Verbose
     # C:\Users\$($_.Name)\AppData\Local\Mozilla\Firefox\Profiles\*.default\cache\*.* -Recurse -Force -EA SilentlyContinue -Verbose
	    #C:\Users\$($_.Name)\AppData\Local\Mozilla\Firefox\Profiles\*.default\cache2\entries\*.* -Recurse -Force -EA SilentlyContinue -Verbose
     # C:\Users\$($_.Name)\AppData\Local\Mozilla\Firefox\Profiles\*.default\thumbnails\* -Recurse -Force -EA SilentlyContinue -Verbose
     # C:\Users\$($_.Name)\AppData\Local\Mozilla\Firefox\Profiles\*.default\cookies.sqlite -Recurse -Force -EA SilentlyContinue -Verbose
     # C:\Users\$($_.Name)\AppData\Local\Mozilla\Firefox\Profiles\*.default\webappsstore.sqlite -Recurse -Force -EA SilentlyContinue -Verbose
     # C:\Users\$($_.Name)\AppData\Local\Mozilla\Firefox\Profiles\*.default\chromeappsstore.sqlite -Recurse -Force -EA SilentlyContinue -Verbose


      $Browser | %{$b=$_; $ItemsToDelete | %{$i = $_
        $pathsTosearchForItemsToDelete += $b + $i + 'RelativePath'
      }}
      $p2 = @()
      $pathsTosearchForItemsToDelete | %{
        $p2 += Get-Variable -name $_ -valueonly
      }
      $pathsTosearchForItemsToDelete | %{ $relpath = $_
        $removeFromThisDir = join-path $env:LOCALAPPDATA $relpath
        # Directory may not exist, OK to skip and keep processing
        if (test-path -path $removeFromThisDir ) {
          if (-not (test-path -path $removeFromThisDir -pathtype Container)) {throw "$removeFromThisDir exists but is not a directory"}
          # ToDo: find specifiic entities to delete based on DaysToDelete
          if ($PSCmdlet.ShouldProcess($removeFromThisDir, "kill $browser and delete *.*")) {
            stop-process -name $browser
            sleep 3
            cd $removethis
            rm "$removethis/*" -force -Verbose -recurse -ErrorAction SilentlyContinue
          }
        }
      }
      write-host "Ending Clear-BrowserCache"    
    }


    write-host "Starting PostBuild powershell script postBuild.ps1 TargetName = $TargetName, Version = $Version"
    Set-NuSpecVersion $TargetName $Version
    Clear-BrowserCache "Firefox" "Cache" -whatif
    write-host "Ending PostBuild powershell script postBuild.ps1"

