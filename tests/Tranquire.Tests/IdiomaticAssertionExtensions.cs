using System;
using System.Linq.Expressions;
using System.Reflection;
using AutoFixture.Idioms;

namespace Tranquire.Tests
{
    public static class IdiomaticAssertionExtensions
    {
        public static void Verify<T>(this IIdiomaticAssertion assertion)
        {
            if (assertion == null)
            {
                throw new ArgumentNullException(nameof(assertion));
            }

            assertion.Verify(typeof(T));
        }

        public static void Verify<T>(this IIdiomaticAssertion assertion, Expression<Func<T, object>> memberSelector)
        {
            if (assertion == null)
            {
                throw new ArgumentNullException(nameof(assertion));
            }
            if (memberSelector == null)
            {
                throw new ArgumentNullException(nameof(memberSelector));
            }

            VerifyExpression(assertion, memberSelector.Body);
        }

        private static void VerifyExpression(IIdiomaticAssertion assertion, Expression expression)
        {
            switch (expression)
            {
                case MemberExpression memberExpression:
                    VerifyMemberExpression(assertion, memberExpression);
                    break;
                case NewArrayExpression arrayExpression:
                    VerifyArrayExpression(assertion, arrayExpression);
                    break;
                default:
                    throw new ArgumentException("The expression is not a valid member selector");
            }
        }

        private static void VerifyArrayExpression(IIdiomaticAssertion assertion, NewArrayExpression arrayExpression)
        {
            foreach (var expression in arrayExpression.Expressions)
            {
                VerifyExpression(assertion, expression);
            }
        }

        private static void VerifyMemberExpression(IIdiomaticAssertion assertion, MemberExpression memberExpression)
        {
            switch (memberExpression.Member)
            {
                case PropertyInfo propertyInfo:
                    assertion.Verify(propertyInfo);
                    break;
                case FieldInfo fieldInfo:
                    assertion.Verify(fieldInfo);
                    break;
                default:
                    throw new ArgumentException("The expression is not a valid member selector");
            }
        }
    }
}
