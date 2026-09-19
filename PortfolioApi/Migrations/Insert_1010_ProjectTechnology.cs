using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(1010, "insert project technologies for default profile")]
public class Insert_1010_ProjectTechnology : Migration
{
    public override void Up()
    {
        Insert.IntoTable("project_technology")
            .Rows([
                // 1 - SetldPay CI/CD Pipeline
                new
                {
                    project_id = 1,
                    tech = "GitHub Actions"
                },
                new
                {
                    project_id = 1,
                    tech = "CI/CD"
                },
                new
                {
                    project_id = 1,
                    tech = "Azure"
                },

                // 2 - InComm Integration API
                new
                {
                    project_id = 2,
                    tech = ".NET"
                },
                new
                {
                    project_id = 2,
                    tech = "Dapper"
                },
                new
                {
                    project_id = 2,
                    tech = "JWT"
                },
                new
                {
                    project_id = 2,
                    tech = "Azure Key Vault"
                },

                // 3 - Banking Integration API
                new
                {
                    project_id = 3,
                    tech = ".NET"
                },
                new
                {
                    project_id = 3,
                    tech = "Entity Framework"
                },
                new
                {
                    project_id = 3,
                    tech = "SQL Server"
                },
                new
                {
                    project_id = 3,
                    tech = "Redis"
                },
                new
                {
                    project_id = 3,
                    tech = "SignalR"
                },
                new
                {
                    project_id = 3,
                    tech = "Azure Key Vault"
                },

                // 4 - Card Program Management API
                new
                {
                    project_id = 4,
                    tech = ".NET"
                },
                new
                {
                    project_id = 4,
                    tech = "Entity Framework"
                },
                new
                {
                    project_id = 4,
                    tech = "SQL Server"
                },
                new
                {
                    project_id = 4,
                    tech = "Frontegg"
                },
                new
                {
                    project_id = 4,
                    tech = "Azure Key Vault"
                },

                // 5 - Card Program Management Portal
                new
                {
                    project_id = 5,
                    tech = "Angular"
                },
                new
                {
                    project_id = 5,
                    tech = "Typescript"
                },
                new
                {
                    project_id = 5,
                    tech = "TailwindCSS"
                },
                new
                {
                    project_id = 5,
                    tech = "PrimeNG"
                },

                // 6 - Partner Card Management APIs
                new
                {
                    project_id = 6,
                    tech = ".NET"
                },
                new
                {
                    project_id = 6,
                    tech = "SQL Server"
                },
                new
                {
                    project_id = 6,
                    tech = "PostgreSQL"
                },
                new
                {
                    project_id = 6,
                    tech = "Azure Key Vault"
                },

                // 7 - Card Management Portal Platform
                new
                {
                    project_id = 7,
                    tech = "Angular"
                },
                new
                {
                    project_id = 7,
                    tech = "Typescript"
                },
                new
                {
                    project_id = 7,
                    tech = "TailwindCSS"
                },
                new
                {
                    project_id = 7,
                    tech = "PrimeNG"
                },

                // 8 - Standalone .NET Services
                new
                {
                    project_id = 8,
                    tech = ".NET"
                },

                // 9 - Heavy Machinery Dealer APIs
                new
                {
                    project_id = 9,
                    tech = ".NET"
                },
                new
                {
                    project_id = 9,
                    tech = "SQL Server"
                },

                // 10 - Heavy Machinery Management App
                new
                {
                    project_id = 10,
                    tech = ".NET"
                },
                new
                {
                    project_id = 10,
                    tech = "Blazor"
                },
                new
                {
                    project_id = 10,
                    tech = "SQL Server"
                },

                // 11 - Heavy Machinery Parts E-Commerce
                new
                {
                    project_id = 11,
                    tech = ".NET"
                },
                new
                {
                    project_id = 11,
                    tech = "Blazor"
                },
                new
                {
                    project_id = 11,
                    tech = "SQL Server"
                },
                new
                {
                    project_id = 11,
                    tech = "Stripe"
                },
                new
                {
                    project_id = 11,
                    tech = "Mailjet"
                },

                // 12 - Educational Battle Game
                new
                {
                    project_id = 12,
                    tech = "Unity3D"
                },
                new
                {
                    project_id = 12,
                    tech = "C#"
                },
                new
                {
                    project_id = 12,
                    tech = ".NET"
                },

                // 13 - Hyper-Casual Games
                new
                {
                    project_id = 13,
                    tech = "Unity3D"
                },
                new
                {
                    project_id = 13,
                    tech = "C#"
                },

                // 14 - Exam Test Application
                new
                {
                    project_id = 14,
                    tech = "Unity3D"
                },
                new
                {
                    project_id = 14,
                    tech = "C#"
                },

                // 15 - Game Content Management System
                new
                {
                    project_id = 15,
                    tech = ".NET"
                },
                new
                {
                    project_id = 15,
                    tech = "Java"
                },
                new
                {
                    project_id = 15,
                    tech = "Python"
                },
                new
                {
                    project_id = 15,
                    tech = "Flask"
                },

                // 16 - Member Networking System
                new
                {
                    project_id = 16,
                    tech = "Laravel"
                },
                new
                {
                    project_id = 16,
                    tech = "PHP"
                },
                new
                {
                    project_id = 16,
                    tech = "MySQL"
                },

                // 17 - E-Commerce Sub-Site
                new
                {
                    project_id = 17,
                    tech = "Laravel"
                },
                new
                {
                    project_id = 17,
                    tech = "PHP"
                },
                new
                {
                    project_id = 17,
                    tech = "MySQL"
                }
            ]);
    }

    public override void Down()
    {
        Delete.FromTable("project_technology")
            .AllRows();
    }
}
