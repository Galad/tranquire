using Microsoft.AspNetCore;
using ToDoList;

var startup = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
startup.Build().Run();
