using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JogadoresApi.Migrations
{
    /// <inheritdoc />
    public partial class TimeVariasLigas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Times_Ligas_LigaId",
                table: "Times");

            migrationBuilder.DropIndex(
                name: "IX_Times_LigaId",
                table: "Times");

            migrationBuilder.CreateTable(
                name: "LigaTime",
                columns: table => new
                {
                    LigasId = table.Column<int>(type: "int", nullable: false),
                    TimesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LigaTime", x => new { x.LigasId, x.TimesId });
                    table.ForeignKey(
                        name: "FK_LigaTime_Ligas_LigasId",
                        column: x => x.LigasId,
                        principalTable: "Ligas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LigaTime_Times_TimesId",
                        column: x => x.TimesId,
                        principalTable: "Times",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LigaTime_TimesId",
                table: "LigaTime",
                column: "TimesId");

            // Preserva as associações já existentes (Time -> Liga) na nova tabela de junção
            migrationBuilder.Sql(
                "INSERT INTO LigaTime (LigasId, TimesId) SELECT LigaId, Id FROM Times WHERE LigaId IS NOT NULL");

            migrationBuilder.DropColumn(
                name: "LigaId",
                table: "Times");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LigaTime");

            migrationBuilder.AddColumn<int>(
                name: "LigaId",
                table: "Times",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Times_LigaId",
                table: "Times",
                column: "LigaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Times_Ligas_LigaId",
                table: "Times",
                column: "LigaId",
                principalTable: "Ligas",
                principalColumn: "Id");
        }
    }
}
