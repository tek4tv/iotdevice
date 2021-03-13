namespace API_GGG.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Audio
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public double? Capacity { get; set; }

        public string AudioKey { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Creator { get; set; }

        public int? GroupAudioID { get; set; }

        public virtual AudioGroup AudioGroup { get; set; }
    }
}
