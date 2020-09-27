using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices.Models
{
    public class ReceiverTransmitter
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string InputSource { get; set; }
    }
}