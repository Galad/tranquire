﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Tranquire.Reporting;

/// <summary>
/// A composite implementation of <see cref="ICanNotify"/>, where the action or question can send a notifiy only if all the inner <see cref="ICanNotify"/> returns true
/// </summary>
public class CompositeCanNotify : ICanNotify, IEnumerable<ICanNotify>
{
    private readonly ICanNotify[] _innerCanNotify;

    /// <summary>
    /// Creates a new instance of <see cref="CompositeCanNotify"/>
    /// </summary>
    /// <param name="innerCanNotify"></param>
    public CompositeCanNotify(params ICanNotify[] innerCanNotify)
    {
        _innerCanNotify = innerCanNotify ?? throw new System.ArgumentNullException(nameof(innerCanNotify));
    }

    /// <inheritsdoc />
    public bool Action<TResult>(IAction<TResult> action) => Array.TrueForAll(_innerCanNotify, c => c.Action(action));

    /// <inheritsdoc />
    public bool Question<TResult>(IQuestion<TResult> question) => Array.TrueForAll(_innerCanNotify, c => c.Question(question));

    IEnumerator IEnumerable.GetEnumerator() => _innerCanNotify.GetEnumerator();
    /// <inheritdoc />
    public IEnumerator<ICanNotify> GetEnumerator() => ((IEnumerable<ICanNotify>)_innerCanNotify).GetEnumerator();
}
