using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.Static
{
    class MyStringBuilder : IMyStringBuilder
    {
        private string data = "";

        public int Length => data.Length;

        public MyStringBuilder()
        {
            data = "";
        }

        public MyStringBuilder(string value)
        {
            data = value;
        }
        public override string ToString()
        {
            return this.data;
        }

        public IMyStringBuilder Append(string value)
        {
            data += value;
            return this;
        }

        public IMyStringBuilder Remove(int start, int length)
        {
            data = data.Remove(start, length);
            return this;
        }

        public IMyStringBuilder Append(object value)
        {
            if (value is string)
            {
                data += value;
            } else
            {
                data += value.ToString();
            }
            return this;
        }
    }
}
