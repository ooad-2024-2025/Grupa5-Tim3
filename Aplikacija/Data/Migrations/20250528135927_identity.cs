using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grupa5Tim3.Data.Migrations
{
    /// <inheritdoc />
    public partial class identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KorisnikAukcija_Korisnik_korisnikID",
                table: "KorisnikAukcija");

            migrationBuilder.DropForeignKey(
                name: "FK_NotifikacijaKorisnik_Korisnik_KorisnikID",
                table: "NotifikacijaKorisnik");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.AddColumn<DateTime>(
                name: "datumRodjenja",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_KorisnikAukcija_AspNetUsers_korisnikID",
                table: "KorisnikAukcija",
                column: "korisnikID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotifikacijaKorisnik_AspNetUsers_KorisnikID",
                table: "NotifikacijaKorisnik",
                column: "KorisnikID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KorisnikAukcija_AspNetUsers_korisnikID",
                table: "KorisnikAukcija");

            migrationBuilder.DropForeignKey(
                name: "FK_NotifikacijaKorisnik_AspNetUsers_KorisnikID",
                table: "NotifikacijaKorisnik");

            migrationBuilder.DropColumn(
                name: "datumRodjenja",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "prezime",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    datumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prezime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Korisnik_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_KorisnikAukcija_Korisnik_korisnikID",
                table: "KorisnikAukcija",
                column: "korisnikID",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotifikacijaKorisnik_Korisnik_KorisnikID",
                table: "NotifikacijaKorisnik",
                column: "KorisnikID",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
