namespace WebSite.Model.DataBaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DepartmentInfo")]
    public partial class DepartmentInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DepartmentInfo()
        {
            ActionInfo_DepartmentInfo = new HashSet<ActionInfo_DepartmentInfo>();
            UserInfo_DepartmentInfo = new HashSet<UserInfo_DepartmentInfo>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        public int? ParentId { get; set; }

        [StringLength(200)]
        public string TreePath { get; set; }

        public short LevelNumber { get; set; }

        public bool IsPass { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActionInfo_DepartmentInfo> ActionInfo_DepartmentInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserInfo_DepartmentInfo> UserInfo_DepartmentInfo { get; set; }
    }
}
