using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(107, "create profile career")]
public class Create_107_ProfileCareer : Migration
{
    public override void Up()
    {
        Create.Table("profile_career")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("profile_id").AsInt32().NotNullable()
            .WithColumn("name").AsString(256).Nullable()
            .WithColumn("position").AsString(256).Nullable()
            .WithColumn("joined").AsDateTime2().Nullable()
            .WithColumn("leaved").AsDateTime2().Nullable();

        Create.ForeignKey("fk_profile_career_profile")
            .FromTable("profile_career").ForeignColumn("profile_id")
            .ToTable("profile").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_career_profile")
            .OnTable("profile_career");
        Delete.Table("profile_career");
    }
}
