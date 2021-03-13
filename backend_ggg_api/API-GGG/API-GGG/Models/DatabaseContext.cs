namespace API_GGG.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext1")
        {
        }

        public virtual DbSet<AudioGroup> AudioGroups { get; set; }
        public virtual DbSet<Audio> Audios { get; set; }
        public virtual DbSet<ContentGroup> ContentGroups { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<PlaylistTime> PlaylistTimes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TotalScenario> TotalScenarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AudioGroup>()
                .HasMany(e => e.Audios)
                .WithOptional(e => e.AudioGroup)
                .HasForeignKey(e => e.GroupAudioID);

            modelBuilder.Entity<Device>()
                .HasMany(e => e.Groups)
                .WithMany(e => e.Devices)
                .Map(m => m.ToTable("GroupDevice").MapLeftKey("DeviceID").MapRightKey("GroupID"));

            modelBuilder.Entity<Device>()
                .HasMany(e => e.TotalScenarios)
                .WithMany(e => e.Devices)
                .Map(m => m.ToTable("TotalScenarioDevice").MapLeftKey("DeviceID").MapRightKey("TotalScenarioID"));

            
        }
    }
}
