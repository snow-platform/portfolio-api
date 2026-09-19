using FluentMigrator;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.Migrations;

[Migration(1006, "insert cv for default profile")]
public class Insert_1006_ProfileCV : Migration
{
    public override void Up()
    {
        Insert.IntoTable("profile_cv")
            .Row(new
            {
                profile_id = 1,
                cv = "https://mega.nz/file/WLQCiLQS#xN5WiTueXo9tAe81yQr3PtS4ejLsbo9gtCKxHeWzAzI",
                created_at = DateTime.Now
            });
    }

    public override void Down()
    {
        Delete.FromTable("profile_cv")
            .Row(new
            {
                profile_id = 1
            });
    }
}
