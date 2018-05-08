using AutoFixture.Idioms;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Tranquire.Tests
{
    public static class IdiomaticAssertionExtensions
    {
        public static void Verify<T>(this IIdiomaticAssertion assertion, Expression<Func<T, object>> memberSelector)
        {
            if (memberSelector == null)
            {
                throw new ArgumentNullException(nameof(memberSelector));
            }

            var memberExpression = memberSelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a valid member selector");
            }

            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                assertion.Verify(propertyInfo);
                return;
            }
            throw new ArgumentException("The expression is not a valid member selector");
        }
    }
}
