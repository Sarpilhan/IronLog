# IronLog.File

.Net Core Simple Logger Extension

## Setup

`Install-Package IronLog.File -Version 1.0.2`

`dotnet add package IronLog.File --version 1.0.2`

`<PackageReference Include="IronLog.File" Version="1.0.2" />`

######  Startup.cs
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddLogging(builder => builder.AddFileLogger(Configuration));
}
```
######  appsettings.json
```json
"IronLogOptions": {
    "LoggerType": "txt", //json 
    "Path": "\\Log",
    "FileNameStatic": "Log_{0}",
    "SplitFormat": "Hourly",  //Infinite, Minute, Hourly, QuarterlyDaily, HalfDay, Daily, Weekly, Monthly
    "Layout": "{date} {level} {logger}  {message} {exception}",
    "DateFormat": "dd/MM/yyyy HH:mm:ss" 
}
```


## Usage
```csharp
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger; 
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Sample 1");
        _logger.LogDebug("Sample 2");
        _logger.LogError("Sample 3");
        _logger.LogTrace("Sample 4");
        _logger.LogCritical("Sample 5");
        return View();
    } 
}
```

