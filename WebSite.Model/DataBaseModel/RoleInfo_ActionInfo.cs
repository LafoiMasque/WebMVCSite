namespace WebSite.Model.DataBaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoleInfo_ActionInfo
    {
        public int Id { get; set; }

        public int ActionInfoId { get; set; }

        public int RoleInfoId { get; set; }

        public virtual ActionInfo ActionInfo { get; set; }

        public virtual RoleInfo RoleInfo { get; set; }
    }
}
