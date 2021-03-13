namespace API_GGG.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TotalScenario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TotalScenario()
        {
            Devices = new HashSet<Device>();
            
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Creator { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string PlaylistTimeID { get; set; }

       

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Device> Devices { get; set; }

       
        
    }
}
