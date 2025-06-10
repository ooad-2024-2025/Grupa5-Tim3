using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grupa5Tim3.Data.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Aukcija_umjetninaID",
                table: "Aukcija",
                column: "umjetninaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Aukcija_Umjetnina_umjetninaID",
                table: "Aukcija",
                column: "umjetninaID",
                principalTable: "Umjetnina",
                principalColumn: "umjetinaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aukcija_Umjetnina_umjetninaID",
                table: "Aukcija");

            migrationBuilder.DropIndex(
                name: "IX_Aukcija_umjetninaID",
                table: "Aukcija");
        }
    }
}
