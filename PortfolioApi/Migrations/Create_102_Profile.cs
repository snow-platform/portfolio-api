using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(102, "Create profile table")]
public class Create_102_Profile : Migration
{
    public override void Up()
    {
        Create.Table("profile")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("external_id").AsString(36).NotNullable()
            .WithColumn("user_id").AsInt32().NotNullable()
            .WithColumn("first_name").AsString(256).Nullable()
            .WithColumn("last_name").AsString(256).Nullable()
            .WithColumn("email").AsString(256).Nullable()
            .WithColumn("photo").AsString(256).Nullable()
            .WithColumn("title").AsString(256).Nullable()
            .WithColumn("stack").AsString(256).Nullable()
            .WithColumn("state").AsString(256).Nullable()
            .WithColumn("about").AsString(int.MaxValue).Nullable()
            .WithColumn("summary").AsString(int.MaxValue).Nullable()
            .WithColumn("created_at").AsDateTime2().NotNullable()
            .WithColumn("updated_at").AsDateTime2().NotNullable();

        Create.ForeignKey("fk_profile_user")
            .FromTable("profile").ForeignColumn("user_id")
            .ToTable("user").PrimaryColumn("id");

        Create.UniqueConstraint("uc_profile_external_id")
            .OnTable("profile")
            .Column("external_id");

        Create.UniqueConstraint("uc_profile_email")
            .OnTable("profile")
            .Column("email");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_user")
            .OnTable("profile");
        Delete.UniqueConstraint("uc_profile_external_id")
            .FromTable("profile");
        Delete.UniqueConstraint("uc_profile_email")
            .FromTable("profile");
        Delete.Table("profile");
    }
}
