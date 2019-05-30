using System;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Tranquire
{
    /// <summary>
    /// Contains extension methods for <see cref="IVerifies"/> that perform verifications with NUnit
    /// </summary>
    public static class VerifiesExtensions
    {
        /// <summary>
        /// Verifies the answer of the given question using a NUnit constraint
        /// </summary>
        /// <typeparam name="T">The answer type</typeparam>
        /// /// <param name="verifies">The <see cref="IVerifies"/> instance</param>
        /// <param name="question">The question to verify the answer from</param>
        /// <param name="constraint">A NUnit constraint that encapsulate the expected result</param>        
        /// <returns>The answer, when the verification succeeds</returns>
        public static T Then<T>(this IVerifies verifies, IQuestion<T> question, IResolveConstraint constraint)
        {
            if (verifies == null)
            {
                throw new ArgumentNullException(nameof(verifies));
            }
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }

            return verifies.Then(question, answer => Assert.That(answer, constraint));
        }

        /// <summary>
        /// Verifies the answer of the given question using a NUnit constraint
        /// </summary>
        /// <typeparam name="T">The answer type</typeparam>
        /// /// <param name="verifies">The <see cref="IVerifies"/> instance</param>
        /// <param name="question">The question to verify the answer from</param>
        /// <param name="constraint">A NUnit constraint that encapsulate the expected result</param>
        /// <param name="getExceptionMessage">A function that returns the message to return when the assertion fails</param>        
        /// <returns>The answer, when the verification succeeds</returns>
        public static T Then<T>(
            this IVerifies verifies,
            IQuestion<T> question,
            IResolveConstraint constraint,
            Func<string> getExceptionMessage)
        {
            if (verifies == null)
            {
                throw new ArgumentNullException(nameof(verifies));
            }
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }
            if (getExceptionMessage == null)
            {
                throw new ArgumentNullException(nameof(getExceptionMessage));
            }

            return verifies.Then(question, answer => Assert.That(answer, constraint, getExceptionMessage));
        }

        /// <summary>
        /// Verifies the answer of the given question using a NUnit constraint
        /// </summary>
        /// <typeparam name="T">The answer type</typeparam>
        /// /// <param name="verifies">The <see cref="IVerifies"/> instance</param>
        /// <param name="question">The question to verify the answer from</param>
        /// <param name="constraint">A NUnit constraint that encapsulate the expected result</param>
        /// <param name="message">The message to return when the assertion fails</param>
        /// <param name="args">Arguments to add to the message</param>
        /// <returns>The answer, when the verification succeeds</returns>
        public static T Then<T>(
            this IVerifies verifies,
            IQuestion<T> question,
            IResolveConstraint constraint,
            string message,
            params object[] args)
        {

            ValidateArguments(verifies, question, constraint, message, args);

            return verifies.Then(question, answer => Assert.That(answer, constraint, message, args));
        }

        /// <summary>
        /// Verifies the answer of the given question using a NUnit constraint
        /// </summary>
        /// <typeparam name="T">The answer type</typeparam>
        /// /// <param name="verifies">The <see cref="IVerifies"/> instance</param>
        /// <param name="question">The question to verify the answer from</param>
        /// <param name="constraint">A NUnit constraint that encapsulate the expected result</param>        
        /// <returns>The answer, when the verification succeeds</returns>
        public static Task<T> Then<T>(this IVerifies verifies, IQuestion<Task<T>> question, IResolveConstraint constraint)
        {
            if (verifies == null)
            {
                throw new ArgumentNullException(nameof(verifies));
            }
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }

            return verifies.Then<T>(question, answer => Assert.That(answer, constraint));
        }

        /// <summary>
        /// Verifies the answer of the given question using a NUnit constraint
        /// </summary>
        /// <typeparam name="T">The answer type</typeparam>
        /// /// <param name="verifies">The <see cref="IVerifies"/> instance</param>
        /// <param name="question">The question to verify the answer from</param>
        /// <param name="constraint">A NUnit constraint that encapsulate the expected result</param>
        /// <param name="getExceptionMessage">A function that returns the message to return when the assertion fails</param>        
        /// <returns>The answer, when the verification succeeds</returns>
        public static Task<T> Then<T>(
            this IVerifies verifies,
            IQuestion<Task<T>> question,
            IResolveConstraint constraint,
            Func<string> getExceptionMessage)
        {
            if (verifies == null)
            {
                throw new ArgumentNullException(nameof(verifies));
            }
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }
            if (getExceptionMessage == null)
            {
                throw new ArgumentNullException(nameof(getExceptionMessage));
            }

            return verifies.Then<T>(question, answer => Assert.That(answer, constraint, getExceptionMessage));
        }

        /// <summary>
        /// Verifies the answer of the given question using a NUnit constraint
        /// </summary>
        /// <typeparam name="T">The answer type</typeparam>
        /// /// <param name="verifies">The <see cref="IVerifies"/> instance</param>
        /// <param name="question">The question to verify the answer from</param>
        /// <param name="constraint">A NUnit constraint that encapsulate the expected result</param>
        /// <param name="message">The message to return when the assertion fails</param>
        /// <param name="args">Arguments to add to the message</param>
        /// <returns>The answer, when the verification succeeds</returns>
        public static Task<T> Then<T>(
            this IVerifies verifies,
            IQuestion<Task<T>> question,
            IResolveConstraint constraint,
            string message,
            params object[] args)
        {
            ValidateArguments(verifies, question, constraint, message, args);

            return verifies.Then<T>(question, answer => Assert.That(answer, constraint, message, args));
        }

        private static void ValidateArguments<T>(IVerifies verifies, IQuestion<T> question, IResolveConstraint constraint, string message, object[] args)
        {
            if (verifies == null)
            {
                throw new ArgumentNullException(nameof(verifies));
            }
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
        }
    }
}
