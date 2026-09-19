using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(111, "create profile certificate")]
public class Create_111_ProfileCertificate : Migration
{
    public override void Up()
    {
        Create.Table("profile_certificate")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("profile_id").AsInt32().NotNullable()
            .WithColumn("name").AsString(256).Nullable()
            .WithColumn("issuer").AsString(256).Nullable()
            .WithColumn("proof").AsString(256).Nullable()
            .WithColumn("issued_at").AsDateTime2().Nullable();

        Create.ForeignKey("fk_profile_certificate_profile")
            .FromTable("profile_certificate").ForeignColumn("profile_id")
            .ToTable("profile").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_profile_certificate_profile")
            .OnTable("profile_certificate");
        Delete.Table("profile_certificate");
    }
}
