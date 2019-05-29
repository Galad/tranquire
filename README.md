tranquire
=========

[![Build status](https://ci.appveyor.com/api/projects/status/qmv6y5576jyvra1q/branch/master?svg=true)](https://ci.appveyor.com/project/Galad/tranquire/branch/master)
[![Quality gate](https://sonarcloud.io/api/project_badges/measure?project=Tranquire&metric=alert_status)](https://sonarcloud.io/dashboard?id=Tranquire)
[![NuGet](https://img.shields.io/nuget/v/Tranquire.svg)](https://www.nuget.org/packages/Tranquire/)



Introduction
------------
Tranquire brings the [screenplay pattern](http://www.infoq.com/articles/Beyond-Page-Objects-Test-Automation-Serenity-Screenplay) to .NET. It is highly inspired by the implementation of [Serenity BDD](http://www.serenity-bdd.info). You can refer to the Serenity documentation to understand how Tranquire works.
Its goal is to better organize UI acceptance tests and make them more maintainable and readable, by applying the single responsability and open-closed principles to UI actions.

Tranquire enables UI actions by provinding an abstraction layer on top of [Selenium](http://www.seleniumhq.org/) in a form of a independant composable actions.

Documentation
---------------
See the [wiki](https://github.com/Galad/tranquire/wiki)

Solution overview 
--------
The solution is composed by Tranquire as well as test projects.

### Tranquire
* Tranquire : the core library
* Tranquire.Selenium : contains the ui actions and questions used to automate a web UI with Selenium.
* Tranquire.NUnit : contains methods that allows to use NUnit constraint in Then actions

### Demo project
* ToDoList : a [todomvc](http://todomvc.com/) web project used to demonstrate tranquire. 
* ToDoList.Automation : the automation framework for the todomvc app.
* ToDoList.Specifications : the acceptance tests for the todomvc app.

### Tests projects
* Tranquire.Tests
* Tranquire.Selenium.Tests
* Tranquire.NUnit.Tests

Building the sources
--------
### Requirements
* .NET core 2.2 SDK
* .NET Framework 4.6.2 SDK
* Google Chrome (for Selenium tests)
* npm (for the Demo app)

Using Visual Studio 2019, building and testing does not require any particular steps in Debug configuration.

### Building
```
dotnet build Tranquire.sln
```

### Running tests
```
dotnet test Tranquire.sln
```

### Running end to end tests of the demo application
The project ToDoList.Specification contains tests at the Api level and the UI level. They are intended to be run is separate test runs.
In order to run the level you want you can change the default value [here](https://github.com/Galad/tranquire/blob/a36d15aa429f1a1f13a7d1ea601c9b06eda9d7a1/tests/ToDoList.Specifications/Setup.cs#L225). Change to either `TestLevel.UI` or `TestLevel.Api`, and rebuild the project.
It is then recommended to filter the tests by the appropriate category (Api or UI).

As an alternative you can configure the level and run the tests from the console:
```
set TEST_LEVEL=Api
dotnet test Tranquire.sln --filter Category=Api
```

[See here](https://github.com/Galad/tranquire/wiki/Demo-application:-ToDoList) for more information about the demo app
