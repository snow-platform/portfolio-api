using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(103, "create profile hero")]
public class Create_103_ProfileHero : Migration
{
    public override void Up()
    {
        Create.Table("profile_hero")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("profile_id").AsInt32().NotNullable()
            .WithColumn("head").AsString(1024).Nullable()
            .WithColumn("text").AsString(1024).Nullable()
            .WithColumn("title").AsString(256).Nullable()
            .WithColumn("state").AsString(256).Nullable()
            .WithColumn("status").AsString(256).Nullable();

        Create.ForeignKey("fk_profile_hero_profile")
            .FromTable("profile_hero").ForeignColumn("profile_id")
            .ToTable("profile").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_hero_profile")
            .OnTable("profile_hero");
        Delete.Table("profile_hero");
    }
}
