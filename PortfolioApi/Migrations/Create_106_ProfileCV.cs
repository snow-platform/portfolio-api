using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(106, "create profile cv")]
public class Create_106_ProfileCV : Migration
{
    public override void Up()
    {
        Create.Table("profile_cv")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("profile_id").AsInt32().NotNullable()
            .WithColumn("cv").AsString(256).Nullable()
            .WithColumn("created_at").AsDateTime2().NotNullable();

        Create.ForeignKey("fk_profile_cv_profile")
            .FromTable("profile_cv").ForeignColumn("profile_id")
            .ToTable("profile").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_cv_profile")
            .OnTable("profile_cv");
        Delete.Table("profile_cv");
    }
}
