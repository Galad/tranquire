﻿using System;

namespace Tranquire;

/// <summary>
/// Represent an executable action requiring an ability
/// </summary>
/// <typeparam name="TAbility">The ability type</typeparam>    
/// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>

#pragma warning disable CS0618, S1133 // Type or member is obsolete
[Obsolete("Prefer using IAction<T> when exposing an action, or inheriting the abstract class IAction<TAbility, T> when implementing one.", false)]
#pragma warning restore CS0618, S1133 // Type or member is obsolete
public interface IAction<TAbility, out TResult> : IGivenCommand<TAbility, TResult>, IWhenCommand<TAbility, TResult>, IAction<TResult>
{
}

/// <summary>
/// Represent an executable action
/// </summary>
/// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
public interface IAction<out TResult> : IGivenCommand<TResult>, IWhenCommand<TResult>
{
}
