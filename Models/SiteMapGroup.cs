using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices.Models
{
    public class SiteMapGroup
    {
            
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int SiteMapID { get; set; }
        public LiveGroup LiveGroup { get; set; }
       
    }
}