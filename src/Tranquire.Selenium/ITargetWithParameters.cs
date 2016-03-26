using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    public interface ITargetWithParameters
    {
        ITarget Of(params object[] parameters);
    }
}
