using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Represent an object using a <see cref="ITarget"/>
    /// </summary>
    public interface ITargeted
    {
        /// <summary>
        /// Gets the target
        /// </summary>
        ITarget Target { get; }
    }
}
