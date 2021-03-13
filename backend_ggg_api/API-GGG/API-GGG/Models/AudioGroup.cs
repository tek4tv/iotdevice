namespace API_GGG.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AudioGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AudioGroup()
        {
            Audios = new HashSet<Audio>();
        }

        public int ID { get; set; }

        public string Name { get; set; }
        public int FileNumber { get; set; }
        public string DataNumber { get; set; }
        public string AudioPlaylist { get; set; }

        public string Creator { get; set; }

        public DateTime? CreatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Audio> Audios { get; set; }
    }
}
