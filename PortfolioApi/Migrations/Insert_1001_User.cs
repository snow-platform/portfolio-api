using FluentMigrator;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.Migrations;

// format: migration number from 1000+ and 1500 is insertion, update, and deletion of records
[Migration(1001, "insert default user")]
public class Insert_1001_User : Migration
{
    public override void Up()
    {
        Insert.IntoTable("user")
            .Row(new
            {
                email = "estellise.caballero@gmail.com",
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            });
    }

    public override void Down()
    {
        Delete.FromTable("user")
            .Row(new
            {
                email = "estellise.caballero@gmail.com"
            });
    }
}
