using OpenQA.Selenium;
using System;
using System.Diagnostics;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    ///  Represent a target with parameters located by a <see cref="By"/> object
    /// </summary>
    [DebuggerDisplay("Parametarized target : {Name}. Value : {Value}")]
    public sealed class TargetByParameterizable<T> : ITargetWithParameters<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TargetByParameterizable"/>
        /// </summary>
        /// <param name="name">The target name</param>
        /// <param name="createBy">A function that creates the By instance</param>
        public TargetByParameterizable(string name, Func<T, By> createBy)
            :this(_ => name, createBy)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), ExceptionMessages.ArgumentCannotBeNullOrEmpty);
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="TargetByParameterizable"/>
        /// </summary>
        /// <param name="name">A function that returns the name of the target</param>
        /// <param name="createBy">A function that creates the By instance</param>
        private TargetByParameterizable(Func<T, string> name, Func<T, By> createBy)
        {
            CreateBy = createBy ?? throw new ArgumentNullException(nameof(createBy));
            Name = name;
        }

        /// <summary>
        /// Gets the function used to create the locator
        /// </summary>
        public Func<T, By> CreateBy { get; }

        /// <summary>
        /// Gets the target name
        /// </summary>
        public Func<T, string> Name { get; }

        /// <summary>
        /// Returns a target with the  given parameters
        /// </summary>
        /// <param name="parameter">The parameter used to create the By</param>
        /// <returns></returns>
        public ITarget Of(T parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return new TargetBy(CreateBy(parameter), Name(parameter));
        }
    }
}
