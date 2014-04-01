using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dive.Script
{
    public class PropertyConVar : IConVar
    {
        public virtual string Value
        {
            get
            {
                if (!this.Property.CanRead)
                    throw new AccessViolationException("Unable to access get property of variable");

                return this.Property.GetValue(this.Object).ToString();
            }

            set
            {
                if (!this.Property.CanWrite)
                    throw new AccessViolationException("Unable to access set property of variable");

                this.Property.SetValue(this.Object, value);
            }
        }

        public object Object
        {
            get;
            set;
        }

        public PropertyInfo Property
        {
            get;
            set;
        }
    }
}
