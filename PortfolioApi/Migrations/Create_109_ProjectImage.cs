using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(109, "create project image")]
public class Create_109_ProjectImage : Migration
{
    public override void Up()
    {
        Create.Table("project_image")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("project_id").AsInt32().NotNullable()
            .WithColumn("imij").AsString(256).Nullable();

        Create.ForeignKey("fk_project_image_career_project")
            .FromTable("project_image").ForeignColumn("project_id")
            .ToTable("career_project").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_project_image_career_project")
            .OnTable("project_image");
        Delete.Table("project_image");
    }
}
