using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(105, "create profile skills")]
public class Create_105_ProfileSkills : Migration
{
    public override void Up()
    {
        Create.Table("profile_skill")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("profile_id").AsInt32().NotNullable()
            .WithColumn("category").AsString(256).Nullable()
            .WithColumn("name").AsString(256).Nullable()
            .WithColumn("proficiency").AsFloat().NotNullable()
            .WithColumn("created_at").AsDateTime2().Nullable();

        Create.ForeignKey("fk_profile_skill_profile")
            .FromTable("profile_skill").ForeignColumn("profile_id")
            .ToTable("profile").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_skill_profile")
            .OnTable("profile_skill");
        Delete.Table("profile_skill");
    }
}
