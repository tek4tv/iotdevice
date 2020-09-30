using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices.Models
{
    public class LiveInputSource
    {       
        public int ID { get; set; }
        public string Name { get; set; }    
        public string Type { get; set; }
        public string Param { get; set; }
        public bool? IsSchedule { get; set; }
        public DateTime? StartInput { get; set; }
        public DateTime? EndInput { get; set; }
    }
}