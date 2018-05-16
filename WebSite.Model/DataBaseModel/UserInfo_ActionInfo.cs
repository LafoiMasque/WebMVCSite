namespace WebSite.Model.DataBaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserInfo_ActionInfo
    {
        public int Id { get; set; }

        public int ActionInfoId { get; set; }

        public int UserInfoId { get; set; }

        public bool IsPass { get; set; }

        public virtual ActionInfo ActionInfo { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
