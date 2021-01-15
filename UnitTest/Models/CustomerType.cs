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
    /// A class which represents the customer_type table.
    /// </summary>
	[Table("customer_type")]
	public partial class CustomerType
	{
		[Key]
		public virtual int ID { get; set; }
		public virtual int ByName { get; set; }
		public virtual DateTime ModifiedAt { get; set; }
		public virtual DateTime CreatedAt { get; set; }
		public virtual DateTime? DeletedAt { get; set; }
	}
}