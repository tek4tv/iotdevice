using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices.Models
{
    public class LiveDeviceCategory
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Descripton { get; set; }
        public System.Nullable<int> ParentID { get; set; }
        public System.Nullable<int> OrderID { get; set; }
        public System.Nullable<bool> IsShow { get; set; }
        [MaxLength(500)]
        public string Icon { get; set; }
        public ICollection<LiveDevice> LiveDevices { get; set; }

    }
}