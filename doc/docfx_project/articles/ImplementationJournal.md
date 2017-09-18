

## Implementation steps

* Create Web App service in Azure
* Enable git account for the Web App service for continuous deployment
* Configure git on local project
  * Initialise git repository from VS Code
  * Commit all changes
  * Add a git remote using the Git URL from Azure:
    $ git remote add azure https://ciprian_git_2016@solarmonitornz.scm.azurewebsites.net:443/solarmonitornz.git
  * Push to master on azure remote to deploy:
    $ git push azure master  
  * From now on, any deployment can be performed from VS Code via Publish button in the status bar or using:
    $ dotnet publish
    Note: Sometimes the publish button (or the Publish action on the context menu in the Git subpanel) is not available, but the CLI dotnet publish works.
  
* Dependency injection
  * All controllers as well as other classes should obtain their dependencies via an IoC (Inversion of Control) container to ensure they are not tightly coupled to their dependencies.
  * .Net Core supports dependency injection natively
  * First, we need to configure the IoC container with the services we need. This is done in Startup.ConfigureServices().
  * Example - a logging object is needed everywhere and is an ideal target for Dependency Injection. Also, ASP.Net Core supports a logging interface to allow plugging in your favourite logger.
      * In Startup.ConfigureServices(), adding this line will configure the IoC container to generate a logging object when needed:
        services.AddLogging();
      * In a Controller's constructor, simply pass in a parameter such as:
        ILogger<SolarRecordsController> logger
        This parameter will be instantiated automatically by .Net Core's IoC container.

* Database support
  * We will use SQLite as the database backend. We might migrate to MySQL in the future (when a MySQL provider for EF Core is available).  
  * For an Object Relational Mapper (ORM): Entity Framework Core is becoming the standard ORM, has support for code-first and is actively maintained by Microsoft so it's the obvious choice.
  *  
 
      
