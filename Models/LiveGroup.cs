using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices.Models
{
    public class LiveGroup
    {
        public LiveGroup()
        {
            this.LiveDevices = new HashSet<LiveDevice>();
            this.LivePlaylists = new HashSet<LivePlaylist>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.Nullable<int> ParentID { get; set; }
        public System.Nullable<int> OrderID { get; set; }
        public System.Nullable<bool> IsShow { get; set; }      
        public string Icon { get; set; }
        public string InputSource { get; set; }
        public virtual ICollection<LiveDevice> LiveDevices { get; set; }
        public virtual ICollection<LivePlaylist> LivePlaylists { get; set; }
        public ICollection<SiteMapGroup> SiteMapGroups { get; set; }
    }
}