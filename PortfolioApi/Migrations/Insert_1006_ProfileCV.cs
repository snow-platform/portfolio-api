using FluentMigrator;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.Migrations;

[Migration(1006, "insert cv for default profile")]
public class Insert_1006_ProfileCV : Migration
{
    public override void Up()
    {
        Insert.IntoTable("ProfileCV")
            .Row(new
            {
                ProfileId = 1,
                CV = "https://mega.nz/file/WLQCiLQS#xN5WiTueXo9tAe81yQr3PtS4ejLsbo9gtCKxHeWzAzI",
                CreatedAt = DateTime.Now
            });
    }

    public override void Down()
    {
        Delete.FromTable("ProfileCV")
            .Row(new
            {
                ProfileId = 1
            });
    }
}
