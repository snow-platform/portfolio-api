using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(108, "create career project")]
public class Create_108_CareerProject : Migration
{
    public override void Up()
    {
        Create.Table("career_project")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("career_id").AsInt32().NotNullable()
            .WithColumn("title").AsString(256).Nullable()
            .WithColumn("description").AsString(int.MaxValue).Nullable()
            .WithColumn("significance").AsFloat().NotNullable();

        Create.ForeignKey("fk_career_project_profile_career")
            .FromTable("career_project").ForeignColumn("career_id")
            .ToTable("profile_career").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_career_project_profile_career")
            .OnTable("career_project");
        Delete.Table("career_project");
    }
}
