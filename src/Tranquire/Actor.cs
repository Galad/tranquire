﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Tranquire;

/// <summary>
/// Represent an actor which use the system under test. The actor can be given capabilities, such as browsing the web, with the method <see cref="CanUse{T}(T)"/>
/// </summary>
public class Actor : IActorFacade
{
    /// <summary>
    /// Returns the abilities for this actor
    /// </summary>
    public IReadOnlyDictionary<Type, object> Abilities { get; }

    /// <summary>
    /// Gets the actor name.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets the function used to decorate an actor
    /// </summary>
    public Func<IActor, IActor> InnerActorBuilder { get; }

    /// <summary>
    /// Create a new instance of <see cref="Actor"/>
    /// </summary>
    /// <param name="name">The actor's name</param>
    /// <param name="abilities">A dictionary containing the abilities for the actor</param>
    /// <param name="innerActorBuilder">A function called when calling <see cref="IActionExecutor.When{TResult}(IWhenCommand{TResult})"/>,         
    /// <see cref="IActionExecutor.Given{TResult}(IGivenCommand{TResult})"/>,         
    /// Allow the object instanciator to decorate the actor that will be used when calling <see cref="IActor.Execute{TResult}(IAction{TResult})"/> and <see cref="IActor.ExecuteWithAbility{TAbility, TResult}(IAction{TAbility, TResult})"/>
    /// </param>
    public Actor(
        string name,
        IReadOnlyDictionary<Type, object> abilities,
        Func<IActor, IActor> innerActorBuilder)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Abilities = abilities ?? throw new ArgumentNullException(nameof(abilities));
        InnerActorBuilder = innerActorBuilder ?? throw new ArgumentNullException(nameof(innerActorBuilder));
    }

    /// <summary>
    /// Create a new instance of <see cref="Actor"/>
    /// </summary>
    /// <param name="name">The actor's name</param>
    /// <param name="abilities">A dictionary containing the abilities for the actor</param>
    public Actor(string name, IReadOnlyDictionary<Type, object> abilities)
        : this(name, abilities, a => a)
    {
    }

    /// <summary>
    /// Create a new instance of <see cref="Actor"/> without abilities
    /// </summary>
    /// <param name="name">The actor's name</param>
    public Actor(string name) : this(name, new Dictionary<Type, object>()) { }

    /// <summary>
    /// Create a new instance of <see cref="Actor"/> without abilities
    /// </summary>
    /// <param name="name">The actor's name</param>
    /// <param name="innerActorBuilder">A function called when calling <see cref="IActionExecutor.When{TResult}(IWhenCommand{TResult})"/>,         
    /// <see cref="IActionExecutor.Given{TResult}(IGivenCommand{TResult})"/>,         
    /// Allow the object instanciator to decorate the actor that will be used when calling <see cref="IActor.Execute{TResult}(IAction{TResult})"/> and <see cref="IActor.ExecuteWithAbility{TAbility, TResult}(IAction{TAbility, TResult})"/>
    /// </param>
    public Actor(string name, Func<IActor, IActor> innerActorBuilder)
        : this(name, new Dictionary<Type, object>(), innerActorBuilder)
    {
    }

    /// <summary>
    /// Retrieve an actor's ability
    /// </summary>
    /// <param name="abilityType">The type of ability to retrieve</param>        
    /// <returns>The ability</returns>
    /// <exception cref="InvalidOperationException">The actor does not have the requested ability</exception>
    private object AbilityTo(Type abilityType)
    {
        if (!Abilities.TryGetValue(abilityType, out var ability))
        {
            throw new InvalidOperationException($"The ability {abilityType.Name} was requested but the actor {Name} does not have it.");
        }

        var type = ability.GetType();
        if (IsLazyType(type))
        {
            return type.GetProperty("Value").GetValue(ability);
        }
        return ability;
    }

    /// <summary>
    /// Execute the action in the When context
    /// </summary>
    /// <param name="command">The command to execute</param>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public TResult When<TResult>(IWhenCommand<TResult> command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        return CreateWhenActor().Execute(command.AsAction());
    }

    /// <summary>
    /// Execute the action in the Given context
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public TResult Given<TResult>(IGivenCommand<TResult> command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        var actor = CreateGivenActor();
        var commandAction = command.AsAction();
        return actor.Execute(commandAction);
    }

    private IActor CreateGivenActor()
    {
        var givenActor = new GivenInnerActor(Name, AbilityTo);
        var actor = InnerActorBuilder(givenActor);
        givenActor.Actor = actor;
        return actor;
    }

    private IActor CreateWhenActor()
    {
        var whenActor = new WhenInnerActor(Name, AbilityTo);
        var actor = InnerActorBuilder(whenActor);
        whenActor.Actor = actor;
        return actor;
    }

    /// <summary>
    /// Give an ability to the actor
    /// </summary>
    /// <typeparam name="T">The type of the ability</typeparam>
    /// <param name="doSomething">Ability</param>
    /// <returns>A new actor with the given ability</returns>
    public IActorFacade CanUse<T>(T doSomething) where T : class
    {
        if (doSomething == null)
        {
            throw new ArgumentNullException(nameof(doSomething));
        }

        var abilities = Abilities.Concat(new[] { new KeyValuePair<Type, object>(GetAbilityType(), doSomething) })
                                 .ToDictionary(k => k.Key, k => k.Value);
        return new Actor(Name, abilities, InnerActorBuilder);

        static Type GetAbilityType()
        {
            var type = typeof(T);
            if (IsLazyType(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }
    }

    /// <summary>
    /// Ask a question about the current state of the system
    /// </summary>
    /// <typeparam name="TAnswer">Type answer's type</typeparam>
    /// <param name="question">A <see cref="IQuestion{TAnswer}"/> instance representing the question to ask</param>
    /// <returns>The answer to the question.</returns>
    public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
    {
        if (question == null)
        {
            throw new ArgumentNullException(nameof(question));
        }

        return CreateWhenActor().AsksFor(question);
    }

    /// <inheritdoc />
    public TResult Then<TAnswer, TResult>(IQuestion<TAnswer> question, Func<TAnswer, TResult> verifyAction)
    {
        return CreateWhenActor().Execute(new ThenAction<TAnswer, TResult>(question, verifyAction));
    }

    private sealed class WhenInnerActor : InnerActor
    {
        public WhenInnerActor(string name, Func<Type, object> getAbility) : base(name, getAbility)
        {
        }

        public override TResult Execute<TResult>(IAction<TResult> action) => action.ExecuteWhenAs(Actor);
#pragma warning disable CS0618 // Type or member is obsolete
        public override TResult ExecuteWithAbility<TAbility, TResult>(IAction<TAbility, TResult> action) => action.ExecuteWhenAs(Actor, (TAbility)GetAbility(typeof(TAbility)));
#pragma warning restore CS0618 // Type or member is obsolete
    }

    private sealed class GivenInnerActor : InnerActor
    {
        public GivenInnerActor(string name, Func<Type, object> getAbility) : base(name, getAbility)
        {
        }

        public override TResult Execute<TResult>(IAction<TResult> action) => action.ExecuteGivenAs(Actor);
#pragma warning disable CS0618 // Type or member is obsolete
        public override TResult ExecuteWithAbility<TAbility, TResult>(IAction<TAbility, TResult> action) => action.ExecuteGivenAs(Actor, (TAbility)GetAbility(typeof(TAbility)));
#pragma warning restore CS0618 // Type or member is obsolete
    }

    private abstract class InnerActor : IActor
    {
        protected InnerActor(
            string name,
            Func<Type, object> getAbility)
        {
            Name = name;
            GetAbility = getAbility;
        }

        public IActor Actor { get; set; }
        public string Name { get; }
        public Func<Type, object> GetAbility { get; }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return question.AnsweredBy(Actor);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TAnswer AsksForWithAbility<TAbility, TAnswer>(IQuestion<TAbility, TAnswer> question)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            return question.AnsweredBy(Actor, (TAbility)GetAbility(typeof(TAbility)));
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public abstract TResult ExecuteWithAbility<TAbility, TResult>(IAction<TAbility, TResult> action);
#pragma warning restore CS0618 // Type or member is obsolete
        public abstract TResult Execute<TResult>(IAction<TResult> action);
    }

    private static bool IsLazyType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Lazy<>);
    }
}
