using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(113, "create profile cms")]
public class Create_113_ProfileCms : Migration
{
    public override void Up()
    {
        Create.Table("ProfileCms")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("ProfileId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(256).Nullable()
            .WithColumn("Token").AsString(int.MaxValue).Nullable()
            .WithColumn("CreatedAt").AsDateTime2().NotNullable();

        Create.ForeignKey("FK_ProfileCms_Profile")
            .FromTable("ProfileCms").ForeignColumn("ProfileId")
            .ToTable("Profile").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_ProfileCms_Profile")
            .OnTable("ProfileCms");
        Delete.Table("ProfileCms");
    }
}
