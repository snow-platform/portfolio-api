using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(112, "create profile education")]
public class Create_112_ProfileEducation : Migration
{
    public override void Up()
    {
        Create.Table("profile_education")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("profile_id").AsInt32().NotNullable()
            .WithColumn("degree").AsString(256).Nullable()
            .WithColumn("degree_abbrev").AsString(256).Nullable()
            .WithColumn("field_of_study").AsString(256).Nullable()
            .WithColumn("field_of_study_abbrev").AsString(256).Nullable()
            .WithColumn("school").AsString(256).Nullable()
            .WithColumn("enrolled").AsDateTime2().Nullable()
            .WithColumn("graduated").AsDateTime2().Nullable();

        Create.ForeignKey("fk_profile_education_profile")
            .FromTable("profile_education").ForeignColumn("profile_id")
            .ToTable("profile").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_education_profile")
            .OnTable("profile_education");
        Delete.Table("profile_education");
    }
}
