using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(1005, "insert skills for default profile")]
public class Insert_1005_ProfileSkill : Migration
{
    public override void Up()
    {
        Insert.IntoTable("profile_skill")
            .Rows([
                new
                {
                    profile_id = 1,
                    category = "Languages",
                    name = "C#",
                    proficiency = 0.95f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Languages",
                    name = "Typescript",
                    proficiency = 0.75f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Languages",
                    name = "JavaScript",
                    proficiency = 0.75f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Languages",
                    name = "SQL",
                    proficiency = 0.7f,
                    created_at = DateTime.Now
                },

                new
                {
                    profile_id = 1,
                    category = "Frameworks",
                    name = ".NET",
                    proficiency = 0.95f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Frameworks",
                    name = "ASP.NET Core",
                    proficiency = 0.9f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Frameworks",
                    name = "Blazor",
                    proficiency = 0.65f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Frameworks",
                    name = "Angular",
                    proficiency = 0.6f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Frameworks",
                    name = "Node.js",
                    proficiency = 0.7f,
                    created_at = DateTime.Now
                },

                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "Entity Framework / EF Core",
                    proficiency = 0.85f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "Dapper",
                    proficiency = 0.85f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "CI/CD",
                    proficiency = 0.9f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "Git",
                    proficiency = 0.85f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "GitHub",
                    proficiency = 0.85f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "GitHub Actions",
                    proficiency = 0.75f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "Docker",
                    proficiency = 0.75f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "Azure",
                    proficiency = 0.7f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "Google Cloud Services",
                    proficiency = 0.6f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "SQL Server",
                    proficiency = 0.8f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "PostgreSQL",
                    proficiency = 0.75f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "MySQL",
                    proficiency = 0.7f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "Infra & Data",
                    name = "SQLite",
                    proficiency = 0.85f,
                    created_at = DateTime.Now
                },

                new
                {
                    profile_id = 1,
                    category = "IDE",
                    name = "Rider",
                    proficiency = 0.9f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "IDE",
                    name = "WebStorm",
                    proficiency = 0.9f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "IDE",
                    name = "DataGrip",
                    proficiency = 0.9f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "IDE",
                    name = "VS Code",
                    proficiency = 0.7f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "IDE",
                    name = "Visual Studio",
                    proficiency = 0.7f,
                    created_at = DateTime.Now
                },

                new
                {
                    profile_id = 1,
                    category = "AI",
                    name = "Claude Code",
                    proficiency = 0.85f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "AI",
                    name = "GitHub Copilot",
                    proficiency = 0.8f,
                    created_at = DateTime.Now
                },
                new
                {
                    profile_id = 1,
                    category = "AI",
                    name = "AI Assistant",
                    proficiency = 0.75f,
                    created_at = DateTime.Now
                }
            ]);
    }

    public override void Down()
    {
        Delete.FromTable("profile_skill")
            .Row(new
            {
                profile_id = 1
            });
    }
}
