using FluentMigrator;
using PortfolioApi.Entities.DB;

namespace PortfolioApi.Migrations;

[Migration(1003, "insert hero for profile apply.estellise.caballero@gmail.com")]
public class Insert_1003_ProfileHero : Migration
{
    public override void Up()
    {
        Insert.IntoTable("ProfileHero")
            .Row(new
            {
                ProfileId = 1,
                Head = "I build <<text-primary,\"reliable,\">> <<text-secondary,\"scalable,\">> and <<text-accent,\"maintainable\">> systems",
                Text = "A .NET Developer with 7+ years of experience designing, developing, and maintaining applications and services. <<line-through,text-white,\"I<>wan<>to<>be<>rich!!!\">> Let me know your vision, and I'll show you the craft.",
                Title = ".NET Developer",
                State = "Cebu",
                Status = "Open to work"
            });
    }

    public override void Down()
    {
        Delete.FromTable("ProfileHero")
            .Row(new { ProfileId = 1 });
    }
}