namespace Tek4TV.Devices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LiveDeviceCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Descripton = c.String(),
                        ParentID = c.Int(),
                        OrderID = c.Int(),
                        IsShow = c.Boolean(),
                        Icon = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LiveDevices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IMEI = c.String(),
                        LinkStream = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        LiveCategoryID = c.Int(nullable: false),
                        ExpDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LiveDeviceCategories", t => t.LiveCategoryID, cascadeDelete: true)
                .Index(t => t.LiveCategoryID);
            
            CreateTable(
                "dbo.LiveGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 500),
                        Description = c.String(),
                        ParentID = c.Int(),
                        OrderID = c.Int(),
                        IsShow = c.Boolean(),
                        Icon = c.String(maxLength: 500),
                        InputSource = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LivePlaylists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartPlaylist = c.DateTime(),
                        EndPlaylist = c.DateTime(),
                        Playlist = c.String(),
                        IsPublish = c.Boolean(),
                        IsDelete = c.Boolean(),
                        UniqueName = c.String(maxLength: 20),
                        role = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SiteMapGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GroupID = c.Int(nullable: false),
                        SiteMapID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LiveGroups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.LiveGroupDevice",
                c => new
                    {
                        GroupID = c.Int(nullable: false),
                        DeviceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupID, t.DeviceID })
                .ForeignKey("dbo.LiveGroups", t => t.GroupID, cascadeDelete: true)
                .ForeignKey("dbo.LiveDevices", t => t.DeviceID, cascadeDelete: true)
                .Index(t => t.GroupID)
                .Index(t => t.DeviceID);
            
            CreateTable(
                "dbo.LiveGroupPlaylist",
                c => new
                    {
                        PlaylistID = c.Int(nullable: false),
                        GroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PlaylistID, t.GroupID })
                .ForeignKey("dbo.LivePlaylists", t => t.PlaylistID, cascadeDelete: true)
                .ForeignKey("dbo.LiveGroups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.PlaylistID)
                .Index(t => t.GroupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteMapGroups", "GroupID", "dbo.LiveGroups");
            DropForeignKey("dbo.LiveGroupPlaylist", "GroupID", "dbo.LiveGroups");
            DropForeignKey("dbo.LiveGroupPlaylist", "PlaylistID", "dbo.LivePlaylists");
            DropForeignKey("dbo.LiveGroupDevice", "DeviceID", "dbo.LiveDevices");
            DropForeignKey("dbo.LiveGroupDevice", "GroupID", "dbo.LiveGroups");
            DropForeignKey("dbo.LiveDevices", "LiveCategoryID", "dbo.LiveDeviceCategories");
            DropIndex("dbo.LiveGroupPlaylist", new[] { "GroupID" });
            DropIndex("dbo.LiveGroupPlaylist", new[] { "PlaylistID" });
            DropIndex("dbo.LiveGroupDevice", new[] { "DeviceID" });
            DropIndex("dbo.LiveGroupDevice", new[] { "GroupID" });
            DropIndex("dbo.SiteMapGroups", new[] { "GroupID" });
            DropIndex("dbo.LiveDevices", new[] { "LiveCategoryID" });
            DropTable("dbo.LiveGroupPlaylist");
            DropTable("dbo.LiveGroupDevice");
            DropTable("dbo.SiteMapGroups");
            DropTable("dbo.LivePlaylists");
            DropTable("dbo.LiveGroups");
            DropTable("dbo.LiveDevices");
            DropTable("dbo.LiveDeviceCategories");
        }
    }
}
