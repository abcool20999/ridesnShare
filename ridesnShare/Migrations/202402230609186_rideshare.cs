namespace ridesnShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rideshare : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Passengers", "username", c => c.String());
            AddColumn("dbo.Passengers", "password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Passengers", "password");
            DropColumn("dbo.Passengers", "username");
        }
    }
}
