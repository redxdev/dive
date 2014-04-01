using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dive.Script
{
    public class BasicConVar : IConVar
    {
        public virtual string Value
        {
            get;
            set;
        }

        public BasicConVar(string value)
        {
            this.Value = value;
        }

        public BasicConVar()
        {
        }
    }
}
