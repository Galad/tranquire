tranquire
=========

Introduction
------------
Tranquire brings the [screenplay pattern](http://www.infoq.com/articles/Beyond-Page-Objects-Test-Automation-Serenity-Screenplay) to .NET. It is highly inspired by the implementation of [Serenity BDD](http://www.serenity-bdd.info). You can refer to the Serenity documentation to understand how Tranquire works.
Its goal is to better organize UI acceptance tests and make them more maintainable and readable, by applying the single responsability and open-closed principles to UI actions.

Tranquire provides an abstraction layer on top of [Selenium](http://www.seleniumhq.org/).

Getting started
---------------
The core concept of the screenplay pattern is the Actor. An Actor represents typically a user, but more widely it represents any entity exercising the system. In Tranquire, an actor is represented with the `Actor` class.

An actor has capabilities, such as using a web browser to connect to the tested web site, or making http queries in order to consume a REST web service. Basically, a capability is an abstraction of a dependency, such as Selenium, that need to be used to communicate to the system under test from the actual test code. 

So let's create the actor named John

    var john = new Actor("John");

Right now, John cannot do anything, so let's giving him the ability to browse the web with Firefox through Selenium.

    var driver = new FirefoxDriver();
    //do some config on the web driver
    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
    //an Actor is immutable
    john = john.Can(BrowseTheWeb.With(driver));
    
Now that John can browse the web, he can perform Actions. Actions are represented in Tranquire by the interfaces `IAction<TGiven, TWhen>` and `IAction`, depending of wether the actor requires a particular ability.

In Tranquire, Actions are executed in one of the 2 contexts : Given and When. The goal is to allow different implementations of a particular action in different context. For instance, an action can use the web browser in the When context and use an API in the Given context. This approach allows to faster actions in the Given context.

So let's perform an action. First, John needs to open the application

    //Execute the action in the Given context
    john.WasAbleTo(Navigate.ToUrl("http://localhost:57897"));

Then he wants to add an item to the to-do list

    //Execute the action in the When context
    john.AttemptsTo(ToDoItem.AddAToDoItem("Buy some milk"));

The action provided by the ToDoItem class is composed of several other actions.

Finally, we want to verify that the item was added. This is done with Questions. The concept of Questions is represented in Tranquire by the interface `IQuestion<TAnswer>` and `IQuestion<TAnswer, TAbility>`.

    var items = john.AsksFor(TheItems.Displayed());
    Assert.Contains("Buy some milk", items);

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

[![Build status](https://ci.appveyor.com/api/projects/status/qmv6y5576jyvra1q/branch/master?svg=true)](https://ci.appveyor.com/project/Galad/tranquire/branch/master)
