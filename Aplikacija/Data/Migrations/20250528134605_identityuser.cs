using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grupa5Tim3.Data.Migrations
{
    /// <inheritdoc />
    public partial class identityuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "korisnickoIme",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "lozinka",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "uloga",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "verifikovan",
                table: "Korisnik");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Korisnik",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "korisnickoIme",
                table: "Korisnik",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lozinka",
                table: "Korisnik",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "uloga",
                table: "Korisnik",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "verifikovan",
                table: "Korisnik",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
