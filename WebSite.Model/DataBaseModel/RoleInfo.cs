namespace WebSite.Model.DataBaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleInfo")]
    public partial class RoleInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoleInfo()
        {
            RoleInfo_ActionInfo = new HashSet<RoleInfo_ActionInfo>();
            RoleInfo_UserInfo = new HashSet<RoleInfo_UserInfo>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public int? Sort { get; set; }

        public int LastModifierId { get; set; }

        public DateTime LastModifyTime { get; set; }

        public int CreaterId { get; set; }

        public DateTime CreateTime { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public byte? StateFlag { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleInfo_ActionInfo> RoleInfo_ActionInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleInfo_UserInfo> RoleInfo_UserInfo { get; set; }
    }
}
