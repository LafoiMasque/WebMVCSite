namespace WebSite.Model.DataBaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ActionInfo")]
    public partial class ActionInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ActionInfo()
        {
            ActionInfo_DepartmentInfo = new HashSet<ActionInfo_DepartmentInfo>();
            RoleInfo_ActionInfo = new HashSet<RoleInfo_ActionInfo>();
            UserInfo_ActionInfo = new HashSet<UserInfo_ActionInfo>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string ActionName { get; set; }

        [Required]
        [StringLength(200)]
        public string Url { get; set; }

        [Required]
        [StringLength(10)]
        public string HttpMethod { get; set; }

        [StringLength(20)]
        public string ActionMethodName { get; set; }

        [StringLength(20)]
        public string ControllerName { get; set; }

        public byte? ActionTypeEnum { get; set; }

        [StringLength(500)]
        public string MenuIcon { get; set; }

        public double? IconWidth { get; set; }

        public double? IconHeight { get; set; }

        public int? Sort { get; set; }

        public int LastModifierId { get; set; }

        public DateTime LastModifyTime { get; set; }

        public int CreaterId { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public byte? StateFlag { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActionInfo_DepartmentInfo> ActionInfo_DepartmentInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleInfo_ActionInfo> RoleInfo_ActionInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserInfo_ActionInfo> UserInfo_ActionInfo { get; set; }
    }
}
