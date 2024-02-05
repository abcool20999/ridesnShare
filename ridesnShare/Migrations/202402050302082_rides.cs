namespace ridesnShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rides : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        bookingId = c.Int(nullable: false, identity: true),
                        PassengerId = c.Int(nullable: false),
                        tripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.bookingId)
                .ForeignKey("dbo.Passengers", t => t.PassengerId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.tripId, cascadeDelete: true)
                .Index(t => t.PassengerId)
                .Index(t => t.tripId);
            
            CreateTable(
                "dbo.Passengers",
                c => new
                    {
                        passengerId = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        email = c.String(),
                    })
                .PrimaryKey(t => t.passengerId);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        tripId = c.Int(nullable: false, identity: true),
                        startLocation = c.String(),
                        endLocation = c.String(),
                        price = c.String(),
                        Time = c.DateTime(nullable: false),
                        dayOftheweek = c.String(),
                        DriverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.tripId)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverId = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        email = c.String(),
                    })
                .PrimaryKey(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "tripId", "dbo.Trips");
            DropForeignKey("dbo.Trips", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Bookings", "PassengerId", "dbo.Passengers");
            DropIndex("dbo.Trips", new[] { "DriverId" });
            DropIndex("dbo.Bookings", new[] { "tripId" });
            DropIndex("dbo.Bookings", new[] { "PassengerId" });
            DropTable("dbo.Drivers");
            DropTable("dbo.Trips");
            DropTable("dbo.Passengers");
            DropTable("dbo.Bookings");
        }
    }
}
