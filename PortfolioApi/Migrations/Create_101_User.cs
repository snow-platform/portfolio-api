using FluentMigrator;

namespace PortfolioApi.Migrations;

// note
//  format: 100+ table creation
[Migration(101, "Create user table")]
public class Create_101_User : Migration
{
    public override void Up()
    {
        Create.Table("user")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("email").AsString(256).Nullable()
            .WithColumn("created_at").AsDateTime2().NotNullable()
            .WithColumn("updated_at").AsDateTime2().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("user");
    }
}
