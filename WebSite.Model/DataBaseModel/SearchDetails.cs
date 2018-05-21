namespace WebSite.Model.DataBaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SearchDetails
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string KeyWords { get; set; }

        public DateTime SearchTime { get; set; }
    }
}
