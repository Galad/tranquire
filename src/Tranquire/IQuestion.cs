using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent a question
    /// </summary>
    /// <typeparam name="TAnswer">The answer's type</typeparam>
    public interface IQuestion<TAnswer>
    {
        /// <summary>
        /// Answers the question
        /// </summary>
        /// <param name="actor">The actor used to answer the question</param>
        /// <returns>The answer to the question</returns>
        TAnswer AnsweredBy(IActor actor);
    }
}
