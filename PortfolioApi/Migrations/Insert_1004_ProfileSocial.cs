using FluentMigrator;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.Migrations;

[Migration(1004, "insert socials for profile apply.estellise.caballero@gmail.com")]
public class InsertSocial_1004 : Migration
{
    public override void Up()
    {
        Insert.IntoTable("profile_social")
            .Rows([
                new
                {
                    profile_id = 1,
                    name = "LinkedIn",
                    link = "https://linkedin.com/in/carlocaballero"
                },
                new
                {
                    profile_id = 1,
                    name = "GitHub",
                    link = "https://github.com/estellise-yukihime"
                },
                new
                {
                    profile_id = 1,
                    name = "Email",
                    link = "mailto:apply.estellise.caballero@gmail.com"
                }
            ]);
    }

    public override void Down()
    {
        Delete.FromTable("profile_social")
            .Row(new { profile_id = 1 });
    }
}
