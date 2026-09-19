using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(104, "create profile socials")]
public class Create_104_ProfileSocial : Migration
{
    public override void Up()
    {
        Create.Table("profile_social")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("profile_id").AsInt32().NotNullable()
            .WithColumn("name").AsString(256).Nullable()
            .WithColumn("link").AsString(256).Nullable();

        Create.ForeignKey("fk_profile_social_profile")
            .FromTable("profile_social").ForeignColumn("profile_id")
            .ToTable("profile").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_social_profile")
            .OnTable("profile_social");
        Delete.Table("profile_social");
    }
}
