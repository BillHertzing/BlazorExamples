# ReadMe for Demo10 Server

Documentation for the Server used in Demo10 can be found here: [Demo10 Server Documentation](Documentation/Details.html)

## Installing the genericHost as a Windows service
To be a Windows Service requires a number of things to be configured in Windows. These demos will document two manual tools and an full-blown installer to accomplish this

### sc.exe
sc.exe can be used to install the service. 
// ToDo: add links to the history of sc.exe, including what versions of Windows it is available on
// ToDo: add links to instructions for finding and if needed updating sc.exe

### Create the service
1. Open a PowerShell command prompt
1. Navigate to the _PublishedAgetn/PublishedService subdirectory
1. On the command line, enter the following command:  sc.exe create GenericHostAsService binPath="FullPathToTheLocationOfTheServerExe"
	A. For the demos, the path to the Server.exe is found under the `$SolutionDir/Demos/DemoXX/_PublishedAgent/PublishedService
	A. You should see the response `[SC] CreateService SUCCESS`
1. On the command line, enter the following command:  sc.exe start GenericHostAsService
	A. You should see something llike this:
	``` 
	SERVICE_NAME: GenericHostAsService
        TYPE               : 10  WIN32_OWN_PROCESS
        STATE              : 2  START_PENDING
                                (NOT_STOPPABLE, NOT_PAUSABLE, IGNORES_SHUTDOWN)
        WIN32_EXIT_CODE    : 0  (0x0)
        SERVICE_EXIT_CODE  : 0  (0x0)
        CHECKPOINT         : 0x0
        WAIT_HINT          : 0x7d0
        PID                : 15748
        FLAGS              :
	```
1. On the command line, enter the following Powershell command to list the state of the service in theprocess table:  
### Start the service

## InstallUtil.exe

### WiX Installer to create .msi

## verifyinng
