tranquire
=========

Introduction
------------
Tranquire bring the [screenplay pattern](http://www.infoq.com/articles/Beyond-Page-Objects-Test-Automation-Serenity-Screenplay) to .NET. It is highly inspired by the implementation of [Serenity BDD](http://www.serenity-bdd.info). You can refer to the Serenity documentation to understand how Tranquire works.
Its goal is to better organize UI acceptance tests and make them more maintainable and readable, by applying the single responsability and open-closed principles to UI actions.

Tranquire provides an abstraction layer on top of [Selenium](http://www.seleniumhq.org/).

Solution
--------
The solution is composed by Tranquire as well as test projects.

###Tranquire
* Tranquire : the core librairy
* Tranquire.Selenium : contains the ui actions and questions used to automate a web UI with Selenium.

###Demo project
* ToDoList : a [todomvc](http://todomvc.com/) web project used to demonstrate tranquire. 
* ToDoList.Automation : the automation framework for the todomvc app.
* ToDoList.Specifications : the acceptance tests for the todomvc app. Please run the ToDoList web project before executing the tests, so that IIS express can start.

###Tests projects
* Tranquire.Tests
* Tranquire.Selenium.Tests
