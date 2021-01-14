// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591

namespace Models.POCO
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the staff table.
    /// </summary>
	[Table("staff")]
	public partial class Staff
	{
		[Key]
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
}