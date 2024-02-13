namespace ridesnShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driver : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Drivers", "CarType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "CarType");
            DropColumn("dbo.Drivers", "Age");
        }
    }
}
