using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    using ORMFramework.Core;
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// A class which represents the account table.
	/// </summary>
	public partial class Account
	{
		public virtual int ID { get; set; }
		public virtual string UserName { get; set; }
		public virtual string Password { get; set; }
		public virtual int Role { get; set; }
		public virtual DateTime CreatedAt { get; set; }
		public virtual DateTime? DeletedAt { get; set; }
		public virtual DateTime ModifiedAt { get; set; }
		public virtual IEnumerable<Staff> Staffs { get; set; }
	}

	/// <summary>
	/// A class which represents the staff table.
	/// </summary>
	public partial class Staff
	{
		public virtual int ID { get; set; }
		public virtual int Account_id { get; set; }
		public virtual string Name { get; set; }
		public virtual DateTime? DOB { get; set; }
		public virtual string IdentityCard { get; set; }
		public virtual string Phone { get; set; }
		public virtual string Email { get; set; }
		public virtual string Address { get; set; }
		public virtual Account Account { get; set; }
	}
	class Test : DbContext
	{
		public Test(string _name) : base(_name)
		{

		}
	}
}
