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
        public int ID { get; set; }
        public string Name { get; set; }
        public string Descripton { get; set; }
        public System.Nullable<int> ParentID { get; set; }
        public System.Nullable<int> OrderID { get; set; }
        public System.Nullable<bool> IsShow { get; set; }
        public string Icon { get; set; }
        public ICollection<LiveDevice> LiveDevices { get; set; }

    }
}