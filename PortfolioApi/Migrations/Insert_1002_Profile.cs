using FluentMigrator;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.Migrations;

[Migration(1002, "insert default profile")]
public class Insert_1002_Profile : Migration
{
    public override void Up()
    {
        Insert.IntoTable("profile")
            .Row(new
            {
                external_id = "33a88871-2a53-47db-98a3-c479c196f4f5",
                user_id = 1,
                first_name = "Carlo",
                last_name = "Caballero",
                email = "apply.estellise.caballero@gmail.com",
                photo = "https://profile.r2.carlocaballero.com/imijs/portrait/wqe.jpg",
                title = "Senior Software Developer",
                stack = ".NET Developer",
                state = "Cebu",
                about = "A software developer of 7+ years building reliable, maintainable, and scalable systems.",
                summary =
                    "A software developer specializing in C# and .NET technologies, with expertise in building services and APIs. I\ndesign, develop, and maintain applications and APIs that are efficient, reliable, maintainable, and scalable.",
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            });
    }

    public override void Down()
    {
        Delete.FromTable("profile")
            .Row(new { email = "apply.estellise.caballero@gmail.com" });
    }
}
