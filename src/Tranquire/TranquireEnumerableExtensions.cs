﻿using System;
using System.Collections.Generic;
using System.Text;
using Tranquire;

namespace System.Linq
{
    /// <summary>
    /// Extension methods for IEnumerable of actions and questions
    /// </summary>
    public static class TranquireEnumerableExtensions
    {
        /// <summary>
        /// Transform an IEnumerable of action in a single action
        /// </summary>
        /// <param name="actions">The list of actions</param>
        /// <param name="name">The resulting action name</param>
        /// <returns>A new action that will execute the input action one after the other</returns>
        public static IAction<Unit> ToAction(this IEnumerable<IAction<Unit>> actions, string name)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new DefaultCompositeAction(name, actions.ToArray());
        }
    }
}
