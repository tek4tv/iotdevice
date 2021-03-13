namespace API_GGG.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PlaylistTime
    {
       

        public int ID { get; set; }

        public string Name { get; set; }

        public string StartPlaylist { get; set; }

        public string EndPlaylist { get; set; }

        

        public DateTime? CreatedAt { get; set; }

        public string Creator { get; set; }

        public string ContenGroupID { get; set; }

        public string AudioGroupID { get; set; }

       
    }
}
