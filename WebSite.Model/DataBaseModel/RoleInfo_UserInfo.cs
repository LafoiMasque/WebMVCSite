namespace WebSite.Model.DataBaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoleInfo_UserInfo
    {
        public int Id { get; set; }

        public int RoleInfoId { get; set; }

        public int UserInfoId { get; set; }

        public virtual RoleInfo RoleInfo { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
