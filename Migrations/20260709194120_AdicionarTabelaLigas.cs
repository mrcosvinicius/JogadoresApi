using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JogadoresApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelaLigas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LigaId",
                table: "Times",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ligas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ligas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Times_Ligas_LigaId",
                table: "Times");

            migrationBuilder.DropTable(
                name: "Ligas");

            migrationBuilder.DropIndex(
                name: "IX_Times_LigaId",
                table: "Times");

            migrationBuilder.DropColumn(
                name: "LigaId",
                table: "Times");
        }
    }
}
