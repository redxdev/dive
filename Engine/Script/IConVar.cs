using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dive.Script
{
    public interface IConVar
    {
        string Value
        {
            get;
            set;
        }
    }
}
