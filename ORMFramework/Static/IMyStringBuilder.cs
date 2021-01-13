using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMFramework.Static
{
    public interface IMyStringBuilder
    {
        IMyStringBuilder Append(string value);
        IMyStringBuilder Append(object value);
        IMyStringBuilder Remove(int start, int length);
        int Length { get; }
    }
}
