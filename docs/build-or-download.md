# Build or download the AIS client
To get the binary package of the AIS client library, you can either build it yourself or download it from the _Releases_ 
section of this repository.

## Build the client
The AIS client library can be built using MSBuild. Clone the repository in a local folder of yours, navigate to the solution folder and run this command from a developer command prompt:

```cmd
msbuild AisClient.sln  /p:Configuration=Release
```

MsBuild will build the CLI(and Tests) projects and place the executable and it's dependencies in the CLI\bin\Release folder(Tests\bin\Release for the tests project)

Alternatively you can open the solution with Visual Studio and build it from there

## Download the client
The AIS client library can also be downloaded directly, without having to build it yourself. Just head over to the _Releases_ section of
this repository and download the latest version. The package contains the CLI executable file of the AIS client
and all the needed dependencies.
