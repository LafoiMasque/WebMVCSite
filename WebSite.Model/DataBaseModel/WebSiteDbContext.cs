namespace WebSite.Model.DataBaseModel
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class WebSiteDbContext : DbContext
	{
		public WebSiteDbContext()
			: base("name=WebSiteDbContext")
		{
		}

		public virtual DbSet<ActionInfo> ActionInfo { get; set; }
		public virtual DbSet<ActionInfo_DepartmentInfo> ActionInfo_DepartmentInfo { get; set; }
		public virtual DbSet<Category> Category { get; set; }
		public virtual DbSet<DepartmentInfo> DepartmentInfo { get; set; }
		public virtual DbSet<JD_Commodity_001> JD_Commodity_001 { get; set; }
		public virtual DbSet<JD_Commodity_002> JD_Commodity_002 { get; set; }
		public virtual DbSet<JD_Commodity_003> JD_Commodity_003 { get; set; }
		public virtual DbSet<JD_Commodity_004> JD_Commodity_004 { get; set; }
		public virtual DbSet<JD_Commodity_005> JD_Commodity_005 { get; set; }
		public virtual DbSet<KeyWordsRank> KeyWordsRank { get; set; }
		public virtual DbSet<RoleInfo> RoleInfo { get; set; }
		public virtual DbSet<RoleInfo_ActionInfo> RoleInfo_ActionInfo { get; set; }
		public virtual DbSet<RoleInfo_UserInfo> RoleInfo_UserInfo { get; set; }
		public virtual DbSet<SearchDetails> SearchDetails { get; set; }
		public virtual DbSet<UserInfo> UserInfo { get; set; }
		public virtual DbSet<UserInfo_ActionInfo> UserInfo_ActionInfo { get; set; }
		public virtual DbSet<UserInfo_DepartmentInfo> UserInfo_DepartmentInfo { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ActionInfo>()
				.HasMany(e => e.ActionInfo_DepartmentInfo)
				.WithRequired(e => e.ActionInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ActionInfo>()
				.HasMany(e => e.RoleInfo_ActionInfo)
				.WithRequired(e => e.ActionInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ActionInfo>()
				.HasMany(e => e.UserInfo_ActionInfo)
				.WithRequired(e => e.ActionInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Category>()
				.Property(e => e.Code)
				.IsUnicode(false);

			modelBuilder.Entity<Category>()
				.Property(e => e.ParentCode)
				.IsUnicode(false);

			modelBuilder.Entity<Category>()
				.Property(e => e.Url)
				.IsUnicode(false);

			modelBuilder.Entity<DepartmentInfo>()
				.HasMany(e => e.ActionInfo_DepartmentInfo)
				.WithRequired(e => e.DepartmentInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<DepartmentInfo>()
				.HasMany(e => e.UserInfo_DepartmentInfo)
				.WithRequired(e => e.DepartmentInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<JD_Commodity_001>()
				.Property(e => e.Url)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_001>()
				.Property(e => e.ImageUrl)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_002>()
				.Property(e => e.Url)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_002>()
				.Property(e => e.ImageUrl)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_003>()
				.Property(e => e.Url)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_003>()
				.Property(e => e.ImageUrl)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_004>()
				.Property(e => e.Url)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_004>()
				.Property(e => e.ImageUrl)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_005>()
				.Property(e => e.Url)
				.IsUnicode(false);

			modelBuilder.Entity<JD_Commodity_005>()
				.Property(e => e.ImageUrl)
				.IsUnicode(false);

			modelBuilder.Entity<RoleInfo>()
				.HasMany(e => e.RoleInfo_ActionInfo)
				.WithRequired(e => e.RoleInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<RoleInfo>()
				.HasMany(e => e.RoleInfo_UserInfo)
				.WithRequired(e => e.RoleInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserInfo>()
				.HasMany(e => e.RoleInfo_UserInfo)
				.WithRequired(e => e.UserInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserInfo>()
				.HasMany(e => e.UserInfo_ActionInfo)
				.WithRequired(e => e.UserInfo)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserInfo>()
				.HasMany(e => e.UserInfo_DepartmentInfo)
				.WithRequired(e => e.UserInfo)
				.HasForeignKey(e => e.UserInfo_Id)
				.WillCascadeOnDelete(false);
		}
	}
}
