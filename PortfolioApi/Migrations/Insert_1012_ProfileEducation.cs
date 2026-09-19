using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(1012, "insert education for default profile")]
public class Insert_1012_ProfileEducation : Migration
{
    public override void Up()
    {
        Insert.IntoTable("profile_education")
            .Rows([
                new
                {
                    profile_id = 1,
                    degree = "Bachelor of Science",
                    degree_abbrev = "BS",
                    field_of_study = "Information Technology",
                    field_of_study_abbrev = "IT",
                    school = "Cebu Technological University",
                    enrolled = new DateTime(2015, 5, 1),
                    graduated = new DateTime(2019, 6, 1)
                },
            ]);
    }

    public override void Down()
    {
        Delete.FromTable("profile_education")
            .Row(new { profile_id = 1 });
    }
}
