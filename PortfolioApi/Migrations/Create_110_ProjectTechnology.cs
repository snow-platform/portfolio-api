using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(110, "create project technology")]
public class Create_110_ProjectTechnology : Migration
{
    public override void Up()
    {
        Create.Table("project_technology")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("project_id").AsInt32().NotNullable()
            .WithColumn("tech").AsString(256).Nullable();

        Create.ForeignKey("fk_project_technology_career_project")
            .FromTable("project_technology").ForeignColumn("project_id")
            .ToTable("career_project").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_project_technology_career_project")
            .OnTable("project_technology");
        Delete.Table("project_technology");
    }
}
