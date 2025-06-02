using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grupa5Tim3.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodavanjeOpisaUmjetnina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "opis",
                table: "Umjetnina",
                type: "nvarchar(max)",
                nullable: true);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

           

            migrationBuilder.DropColumn(
                name: "opis",
                table: "Umjetnina");

           
        }
    }
}
