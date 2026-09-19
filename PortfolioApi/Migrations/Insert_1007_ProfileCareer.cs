using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(1007, "insert career for default profile")]
public class Insert_1007_ProfileCareer : Migration
{
    public override void Up()
    {
        Insert.IntoTable("profile_career")
            .Rows([
                new
                {
                    profile_id = 1,
                    name = "Full Scale",
                    position = ".NET Developer",
                    joined = new DateTime(2023, 1, 9),
                    leaved = (DateTime?)null
                },
                new
                {
                    profile_id = 1,
                    name = "FreCre, Inc",
                    position = "Mobile + Game / Backend / .NET Developer",
                    joined = new DateTime(2019, 9, 1),
                    leaved = new DateTime(2022, 12, 29)
                },
                new
                {
                    profile_id = 1,
                    name = "Tudlo Innovation Solutions Inc",
                    position = "Web Developer - Intern",
                    joined = new DateTime(2018, 9, 1),
                    leaved = new DateTime(2019, 4, 1)
                }
            ]);
    }

    public override void Down()
    {
        Delete.FromTable("profile_career")
            .Row(new { profile_id = 1 });
    }
}
