namespace WebSite.Model.DataBaseModel
{
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("UserInfo")]
	public partial class UserInfo
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public UserInfo()
		{
			RoleInfo_UserInfo = new HashSet<RoleInfo_UserInfo>();
			UserInfo_ActionInfo = new HashSet<UserInfo_ActionInfo>();
			UserInfo_DepartmentInfo = new HashSet<UserInfo_DepartmentInfo>();
		}

		public int Id { get; set; }

		[StringLength(50)]
		public string UserName { get; set; }

		[Required]
		[StringLength(100)]
		public string Account { get; set; }

		[Required]
		[StringLength(100)]
		public string UserPassword { get; set; }

		[StringLength(200)]
		public string Email { get; set; }

		[StringLength(20)]
		public string Mobile { get; set; }

		public byte? UserType { get; set; }

		public int? Sort { get; set; }

		public DateTime LastLoginTime { get; set; }

		public int LastModifierId { get; set; }

		public DateTime LastModifyTime { get; set; }

		public int CreaterId { get; set; }

		public DateTime CreateTime { get; set; }

		[StringLength(500)]
		public string Remark { get; set; }

		public byte? StateFlag { get; set; }

		[JsonIgnore]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<RoleInfo_UserInfo> RoleInfo_UserInfo { get; set; }

		[JsonIgnore]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<UserInfo_ActionInfo> UserInfo_ActionInfo { get; set; }

		[JsonIgnore]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<UserInfo_DepartmentInfo> UserInfo_DepartmentInfo { get; set; }
	}
}
