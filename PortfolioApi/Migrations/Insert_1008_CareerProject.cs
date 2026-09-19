using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(1008, "insert career projects for default profile")]
public class Insert_1008_CareerProject : Migration
{
    public override void Up()
    {
        Insert.IntoTable("career_project")
            .Rows([
                // Full Scale
                new
                {
                    career_id =1,
                    title ="SetldPay CI/CD Pipeline",
                    description ="""
                                  I designed and implemented the CI/CD pipeline for SetldPay using GitHub Actions to 
                                  automate checks, builds, tests, and deployment workflows across websites, web APIs, 
                                  and services to Azure servers.
                                  """,
                    significance =0.88f
                },
                new
                {
                    career_id =1,
                    title ="InComm Integration API",
                    description ="""
                                  I designed and developed the integration layer between InComm and the 
                                  SetldPay Card Program, processing transaction events. 
                                  I architected the transaction processing around a clean, extensible pattern mapping 
                                  incoming InComm message types to dedicated handlers, secured with JWT authentication 
                                  and policy-based authorization.
                                  """,
                    significance =0.91f
                },
                new
                {
                    career_id =1,
                    title ="Banking Integration API",
                    description ="""
                                  I designed and developed an API following clean architecture principles that serves
                                   as the integration layer between SetldPay's internal applications and third-party 
                                   banking providers. 
                                   It manages bank account onboarding, funding, and internal/external transfers while 
                                   abstracting each banking implementation behind a standard interface, 
                                   secured via custom HMAC-based authentication.
                                  """,
                    significance =0.97f
                },
                new
                {
                    career_id =1,
                    title ="Card Program Management API",
                    description ="""
                                  I developed and maintained the API powering the card program management portal, 
                                  enabling internal operations teams to manage physical and virtual cards, 
                                  transactions, fundings, and programs. 
                                  I'm responsible for the the API's architecture, feature development, 
                                  bug fixes, and test coverage.
                                  """,
                    significance =0.89f
                },
                new
                {
                    career_id =1,
                    title ="Card Program Management Portal",
                    description ="""
                                  I collaborated with a team of three developers to build an internal operations 
                                  dashboard that lets support staff, client managers, and administrators oversee 
                                  physical & virtual cards, fundings, card details, and card designs. 
                                  I focus on implementing features aligned with business requirements, bug fixes, 
                                  test coverage and collaborating with the team to improve the application.
                                  """,
                    significance =0.81f
                },
                new
                {
                    career_id =1,
                    title ="Card Management APIs",
                    description ="""
                                  I designed and developed the API powering multiple physical & virtual card management 
                                  portals across partners, architected around a shared base API that partner-specific 
                                  APIs inherit from — enabling new partner APIs to be spun up quickly. 
                                  Features include card lookup, KYC verification and screening, card linking, 
                                  PIN/ACS management, transaction history, and user/profile management.
                                  """,
                    significance =0.9f
                },
                new
                {
                    career_id =1,
                    title ="Card Management Portal",
                    description ="""
                                  I collaborated with a team of three developers to architect a plug-and-play platform 
                                  for physical & virtual card management portals, centralizing common services, 
                                  reusable components, utilities, and business logic into a single source of truth 
                                  for rapid development and deployment across partners.
                                  """,
                    significance =0.82f
                },
                new
                {
                    career_id =1,
                    title ="Standalone .NET Services",
                    description ="""
                                  I developed standalone .NET services that streamline core business 
                                  operations — including XML data extraction, image batch uploading, web scraping, 
                                  high-throughput webhook processing (thousands of records per minute), 
                                  and automated SMS/email client notifications.
                                  """,
                    significance =0.85f
                },
                new
                {
                    career_id =1,
                    title ="Heavy Machinery Dealer APIs",
                    description ="""
                                  I collaborated with a senior developer and senior architect to develop web APIs 
                                  for heavy machinery products using .NET Web API and SQL Server, enabling platform 
                                  integration for partner and third-party dealers.
                                  """,
                    significance =0.80f
                },
                new
                {
                    career_id =1,
                    title ="Heavy Machinery Management App",
                    description ="""
                                  I collaborated with a team of four developers to build and maintain a web application 
                                  for managing new and pre-owned heavy machinery, featuring detailed machine listings, 
                                  a booking system, and tools for generating quotes, brochures, and reports.
                                  """,
                    significance =0.78f
                },
                new
                {
                    career_id =1,
                    title ="Heavy Machinery Parts E-Commerce",
                    description ="""
                                  I developed an e-commerce web application for selling heavy machinery parts, featuring 
                                  product listings with advanced filtering, a mailing writer and sender, 
                                  mailing notifications, product management, and a seamless payment flow.
                                  """,
                    significance =0.65f
                },

                // FreCre, Inc
                new
                {
                    career_id =2,
                    title ="Educational Battle Game",
                    description ="""
                                  I maintained and developed an educational battle game for Japanese clients, 
                                  implementing features such as battle and leveling systems, a stage system, 
                                  an event reward system, social networking, messaging, notifications, 
                                  in-app purchases, and subscription services.
                                  """,
                    significance =0.87f
                },
                new
                {
                    career_id =2,
                    title ="Hyper-Casual Games",
                    description ="""
                                  I developed hyper-casual mobile games in collaboration with Japanese clients and teams, 
                                  building game systems and content with Unity3D and C#.
                                  """,
                    significance =0.85f
                },
                new
                {
                    career_id =2,
                    title ="Exam Test Application",
                    description ="""
                                  I maintained and developed a test application for high-school and college exams, 
                                  delivering features for exam delivery and content management.
                                  """,
                    significance =0.78f
                },
                new
                {
                    career_id =2,
                    title ="Game Content Management System",
                    description ="""
                                  I developed and maintained a game content management system and its supporting 
                                  back-end servers, using .NET, Java, and Python with Flask, 
                                  backed by Datastore for database management.
                                  """,
                    significance =0.82f
                },

                // Tudlo Innovation Solutions Inc
                new
                {
                    career_id =3,
                    title ="Member Networking System",
                    description ="""
                                  I maintained a member networking system, handling system maintenance, bug fixing, 
                                  planning, and feature implementation — most notably a visualization of a member 
                                  referral system and authentication/authorization mechanisms.
                                  """,
                    significance =0.77f
                },
                new
                {
                    career_id =3,
                    title ="E-Commerce Sub-Site",
                    description ="""
                                  I implemented an e-commerce sub-site with a complete checkout flow as part of the 
                                  networking platform.
                                  """,
                    significance =0.55f
                }
            ]);
    }

    public override void Down()
    {
        Delete.FromTable("career_project")
            .AllRows();
    }
}
