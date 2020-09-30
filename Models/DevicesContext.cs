using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Tek4TV.Devices.Models
{
    public class DevicesContext : DbContext
    {
        public DevicesContext(): base("name=DevicesContext")
        { }
       
        public DbSet<LiveDevice> LiveDevices { get; set; }
        public DbSet<LiveGroup> LiveGroups { get; set; }
        public DbSet<LivePlaylist> LivePlaylists { get; set; }
        public DbSet<LiveDeviceCategory> liveDeviceCategories { get; set; }  
        public DbSet<SiteMapGroup> SiteMapGroups { get; set; }
        public DbSet<LiveInputSource> LiveInputSources { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LiveDevice>()
                .HasRequired<LiveDeviceCategory>(s => s.LiveDeviceCategory)
                .WithMany(g => g.LiveDevices)
                .HasForeignKey<int>(s => s.LiveCategoryID);

            modelBuilder.Entity<SiteMapGroup>()
               .HasRequired<LiveGroup>(s => s.LiveGroup)
               .WithMany(g => g.SiteMapGroups)
               .HasForeignKey<int>(s => s.GroupID);

            modelBuilder.Entity<LiveGroup>()
                .HasMany<LiveDevice>(s => s.LiveDevices)
                .WithMany(c => c.LiveGroups)
                .Map(cs =>
                {
                    cs.MapLeftKey("GroupID");
                    cs.MapRightKey("DeviceID");
                    cs.ToTable("LiveGroupDevice");
                });

            modelBuilder.Entity<LivePlaylist>()
               .HasMany<LiveGroup>(s => s.LiveGroups)
               .WithMany(c => c.LivePlaylists)
               .Map(cs =>
               {
                   cs.MapLeftKey("PlaylistID");
                   cs.MapRightKey("GroupID");
                   cs.ToTable("LiveGroupPlaylist");
               });
           
          

        }
    }
}