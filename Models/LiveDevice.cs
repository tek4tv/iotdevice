using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices.Models
{
    public class LiveDevice
    {
        public LiveDevice()
        {
            this.LiveGroups = new HashSet<LiveGroup>();
        }
        public int ID { get; set; }
        public string  IMEI { get; set; }
        public string LinkStream { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LiveCategoryID { get; set; }      
        public System.Nullable<DateTime> ExpDate { get; set; }     
        public LiveDeviceCategory LiveDeviceCategory { get; set; }
        public virtual ICollection<LiveGroup> LiveGroups { get; set; }

    }
}