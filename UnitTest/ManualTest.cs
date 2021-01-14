using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    using ORMFramework.Core;

	class Test : DbContext
	{
		public Test(string _name) : base(_name)
		{

		}
	}
}
