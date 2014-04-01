using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dive.Script
{
    public class ImmutableConVar : IConVar
    {
        private string value = null;

        public virtual string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                throw new AccessViolationException("Variable is immutable");
            }
        }

        public ImmutableConVar(string value)
        {
            this.value = value;
        }
    }
}
