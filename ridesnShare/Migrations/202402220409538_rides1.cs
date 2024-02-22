namespace ridesnShare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rides1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "username", c => c.String());
            AddColumn("dbo.Drivers", "password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "password");
            DropColumn("dbo.Drivers", "username");
        }
    }
}
