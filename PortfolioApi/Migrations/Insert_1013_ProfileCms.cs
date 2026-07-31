using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(1013, "insert strapi cms for default profile")]
public class Insert_1013_ProfileCms : Migration
{
    public override void Up()
    {
        Insert.IntoTable("ProfileCms")
            .Row(new
            {
                ProfileId = 1,
                Name = "strapi",
                Token = "89939c03fd4c4bcd9573c728775dc7b17883efe2f4ef437e464e63de81e3af020629a8556b04a809dfa914f0ab4edaa5d45d53e2d2de48865d2eb4c924ac37260b3fa6e25ac5a1f65d23a81537b0c00536e67ba8ecfc4b544b20c90258576651c039d03ea94e11f89a7238c3492168bfc30c9a03da57ec1dc0c547fbb2dedaa8",
                CreatedAt = DateTime.Now
            });
    }

    public override void Down()
    {
        Delete.FromTable("ProfileCms")
            .Row(new
            {
                ProfileId = 1,
                Name = "strapi"
            });
    }
}
