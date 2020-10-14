# DickinsonBros.Logger
<a href="https://dev.azure.com/marksamdickinson/dickinsonbros/_build/latest?definitionId=35&amp;branchName=master"> <img alt="Azure DevOps builds (branch)" src="https://img.shields.io/azure-devops/build/marksamdickinson/DickinsonBros/35/master"> </a> <a href="https://dev.azure.com/marksamdickinson/dickinsonbros/_build/latest?definitionId=35&amp;branchName=master"> <img alt="Azure DevOps coverage (branch)" src="https://img.shields.io/azure-devops/coverage/marksamdickinson/dickinsonbros/35/master"> </a><a href="https://dev.azure.com/marksamdickinson/DickinsonBros/_release?_a=releases&view=mine&definitionId=17"> <img alt="Azure DevOps releases" src="https://img.shields.io/azure-devops/release/marksamdickinson/b5a46403-83bb-4d18-987f-81b0483ef43e/17/18"> </a><a href="https://www.nuget.org/packages/DickinsonBros.Logger/"><img src="https://img.shields.io/nuget/v/DickinsonBros.Logger"></a>

A logging service that redacts all logs

Features
* Redacts all logs
* Allows for dictionary of variables to be past in that all become first class propertys in the log.
* Ability to add a correlation id that works though async in straight forward fashion
* Allows for improved testability

<h2>Example Usage</h2>

```C#
var data = new Dictionary<string, object>
           {
               { "Username", "DemoUser" },
               { "Password",
@"{
""Password"": ""password""
}"
               }
           };

var message = "Generic Log Message";
var exception = new Exception("Error");

loggingService.LogDebugRedacted(message);
loggingService.LogDebugRedacted(message, data);

loggingService.LogInformationRedacted(message);
loggingService.LogInformationRedacted(message, data);

loggingService.LogWarningRedacted(message);
loggingService.LogWarningRedacted(message, data);

loggingService.LogErrorRedacted(message, exception);
loggingService.LogErrorRedacted(message, exception, data);

```

```
dbug: DickinsonBros.Logger.Runner.Program[1]
      Generic Log Message

dbug: DickinsonBros.Logger.Runner.Program[1]
      Generic Log Message
      Username: DemoUser
      Password: {
        "Password": "***REDACTED***"
      }

info: DickinsonBros.Logger.Runner.Program[1]
      Generic Log Message

info: DickinsonBros.Logger.Runner.Program[1]
      Generic Log Message
      Username: DemoUser
      Password: {
        "Password": "***REDACTED***"
      }

warn: DickinsonBros.Logger.Runner.Program[1]
      Generic Log Message

warn: DickinsonBros.Logger.Runner.Program[1]
      Generic Log Message
      Username: DemoUser
      Password: {
        "Password": "***REDACTED***"
      }

fail: DickinsonBros.Logger.Runner.Program[1]
      Generic Log Message

System.Exception: Error
fail: DickinsonBros.Logger.Runner.Program[1]
      Generic Log Message
      Username: DemoUser
      Password: {
        "Password": "***REDACTED***"
      }
```

Example Runner Included in folder "DickinsonBros.Logger.Runner"

<h2>Setup</h2>

<h3>Add an implementation for dependencys</h3>

<h4>IRedactor</h4>

    https://www.nuget.org/packages/DickinsonBros.Redactor/

<h3>Add Nuget References</h3>

    https://www.nuget.org/packages/DickinsonBros.Logger/
    https://www.nuget.org/packages/DickinsonBros.Logger.Abstractions
    https://www.nuget.org/packages/DickinsonBros.Redactor.Abstractions


<h3>Create Instance With Dependency Injection</h3>

<h4>Add appsettings.json File With Contents</h4>

 ```json  
{
  "RedactorServiceOptions": {
    "PropertiesToRedact": [
      "Password"
    ],
    "RegexValuesToRedact": []
  }
}
 ```    
 
<h4>Code</h4>

```C#        
using DickinsonBros.Logger.Abstractions;
using DickinsonBros.Logger.Extensions;
using DickinsonBros.Redactor.Extensions;
using DickinsonBros.Redactor.Models;
using Microsoft.Extensions.Logging;
...  

var services = new ServiceCollection();   

//Configure Options
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false)

var configuration = builder.Build();
serviceCollection.AddOptions();
serviceCollection.Configure<RedactorServiceOptions>(_configuration.GetSection(nameof(RedactorServiceOptions)));

//Add Logging
services.AddLogging()

//IRedactorService
services.AddRedactorService();

//ILoggingService
services.AddLoggingService();

//Build Service Provider 
using (var provider = services.BuildServiceProvider())
{
   var loggingService = provider.GetRequiredService<ILoggingService>();
}
```    
