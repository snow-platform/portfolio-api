using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(113, "create profile cms")]
public class Create_113_ProfileCms : Migration
{
    public override void Up()
    {
        Create.Table("profile_cms")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("profile_id").AsInt32().NotNullable()
            .WithColumn("name").AsString(256).Nullable()
            .WithColumn("token").AsString(int.MaxValue).Nullable()
            .WithColumn("created_at").AsDateTime2().NotNullable();

        Create.ForeignKey("fk_profile_cms_profile")
            .FromTable("profile_cms").ForeignColumn("profile_id")
            .ToTable("profile").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_cms_profile")
            .OnTable("profile_cms");
        Delete.Table("profile_cms");
    }
}
