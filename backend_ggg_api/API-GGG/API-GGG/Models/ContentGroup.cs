namespace API_GGG.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContentGroup
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? FileNumber { get; set; }

        [StringLength(50)]
        public string DataNumber { get; set; }

        public string Creator { get; set; }

        public string Playlists { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
