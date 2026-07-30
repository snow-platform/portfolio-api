using FluentMigrator;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.Migrations;

[Migration(1004, "insert socials for profile apply.estellise.caballero@gmail.com")]
public class InsertSocial_1004 : Migration
{
    public override void Up()
    {
        Insert.IntoTable("ProfileSocial")
            .Rows([
                new
                {
                    ProfileId = 1,
                    Name = "LinkedIn",
                    Link = "https://linkedin.com/in/carlocaballero"
                },
                new
                {
                    ProfileId = 1,
                    Name = "GitHub",
                    Link = "https://github.com/estellise-yukihime"
                },
                new
                {
                    ProfileId = 1,
                    Name = "Email",
                    Link = "mailto:apply.estellise.caballero@gmail.com"
                }
            ]);
    }

    public override void Down()
    {
        Delete.FromTable("ProfileSocial")
            .Row(new { ProfileId = 1 });
    }
}