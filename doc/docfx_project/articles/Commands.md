## Essential commands

* dotnet restore	                                                              - Resolve all dependencies via NuGet
* dotnet build all	                                                            - Build all projects
* dotnet test test/server/integration/SolarMonitor.Api.IntegrationTests/        - Run SolarMonitor.Api integration tests
* git push azure master                                                         - Publish SolarMonitor.Api to Azure


## VS Code shortcuts

* F7 (build project)
* F8 (run API integration tests)
* F5 (run project)
* F1 (VS Code command prompt)
* Ctrl-Shift-F (Find in all files)
* Ctrl-P (quick find and open file)
* Ctrl-` (Terminal)
 
## dotnet commands

* dotnet new (initialise new .Net Core project)
* dotnet restore (install all dependencies. re-run after updating dependencied in project.json)
* dotnet build (build project)
* dotnet run (run project. ASP.Net projects will start a test web server locally)
* dotnet publish (use git to push source code to remote git repository on Azure. The project will be built automatically on the Azure as part of this deployment step.)
* dotnet test (run tests - for xUnit test projects)

## Yeoman scaffolding tool

* yo aspnet (create a new .NET Core project based on a template)


## Manual Testing

* Connect to (in Chrome or using Fiddler): 
  * http://localhost:5000/api/SolarRecords (run locally, during testing)
  * http://solarmonitornz.azurewebsites.net/api/SolarRecords (latest published version on Azure)
 
