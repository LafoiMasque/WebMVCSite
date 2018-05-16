namespace WebSite.Model.DataBaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserInfo_DepartmentInfo
    {
        public int Id { get; set; }

        public int DepartmentInfoId { get; set; }

        public int UserInfo_Id { get; set; }

        public virtual DepartmentInfo DepartmentInfo { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
