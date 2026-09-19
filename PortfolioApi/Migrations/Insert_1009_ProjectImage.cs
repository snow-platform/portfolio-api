using FluentMigrator;

namespace PortfolioApi.Migrations;

[Migration(1009, "insert project images for default profile")]
public class Insert_1009_ProjectImage : Migration
{
    public override void Up()
    {
        Insert.IntoTable("project_image")
            .Rows([
                // 5 - Card Program Management Portal
                new
                {
                    project_id =5,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/03WorJDF.jpg"
                },
                new
                {
                    project_id =5,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/eySBU8cT.jpg"
                },

                // 7 - Card Management Portal Platform
                new
                {
                    project_id =7,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/7IH2oIWz.jpg"
                },
                new
                {
                    project_id =7,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/9yp22aP_.jpg"
                },
                new
                {
                    project_id =7,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/sve3EIEL.jpg"
                },
                new
                {
                    project_id =7,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/-M3bABXE.jpg"
                },

                // 12 - Educational Battle Game
                new
                {
                    project_id =12,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/es.jpg"
                },
                new
                {
                    project_id =12,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/wc.jpg"
                },

                // 13 - Hyper-Casual Games
                new
                {
                    project_id =13,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/se.jpg"
                },
                new
                {
                    project_id =13,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/nb.jpg"
                },

                // 14 - Exam Test Application
                new
                {
                    project_id =14,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/ai.jpg"
                },
                new
                {
                    project_id =14,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/cs.jpg"
                },
                new
                {
                    project_id =14,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/ge.jpg"
                },
                new
                {
                    project_id =14,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/is.jpg"
                },
                new
                {
                    project_id =14,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/os.jpg"
                },
                new
                {
                    project_id =14,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/tk.jpg"
                },
                new
                {
                    project_id =14,
                    imij ="https://profile.r2.carlocaballero.com/imijs/projeks/uni.jpg"
                }
            ]);
    }

    public override void Down()
    {
        Delete.FromTable("project_image")
            .AllRows();
    }
}
