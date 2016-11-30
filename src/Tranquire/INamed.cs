using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    /// <summary>
    /// Represent an object having a name
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// Gets the name
        /// </summary>
        string Name { get; }
    }
}
