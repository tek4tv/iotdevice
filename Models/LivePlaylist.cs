using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices.Models
{
    public class LivePlaylist
    {
        public LivePlaylist()
        {
            this.LiveGroups = new HashSet<LiveGroup>();
        }
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }
        public System.Nullable<DateTime> StartPlaylist { get; set; }
        public System.Nullable<DateTime> EndPlaylist { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Playlist { get; set; }
        public System.Nullable<bool> IsPublish { get; set; }
        public System.Nullable<bool> IsDelete { get; set; }   
        public virtual ICollection<LiveGroup> LiveGroups { get; set; }
    }
}