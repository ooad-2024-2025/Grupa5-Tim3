using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grupa5Tim3.Data.Migrations
{
    /// <inheritdoc />
    public partial class DrugaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aukcija",
                columns: table => new
                {
                    AukcijaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    umjetninaID = table.Column<int>(type: "int", nullable: false),
                    trenutnaCijena = table.Column<double>(type: "float", nullable: false),
                    pocetakAukcije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    zavrsetakAukcije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    kupacID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aukcija", x => x.AukcijaID);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    korisnickoIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uloga = table.Column<int>(type: "int", nullable: false),
                    verifikovan = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Notifikacija",
                columns: table => new
                {
                    notifikacijaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipObavijesti = table.Column<int>(type: "int", nullable: false),
                    kodQR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    verifikacijskiKod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    umjetninaID = table.Column<int>(type: "int", nullable: false),
                    korisnikID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifikacija", x => x.notifikacijaID);
                });

            migrationBuilder.CreateTable(
                name: "Umjetnina",
                columns: table => new
                {
                    umjetinaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tehnika = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pocetnaCijena = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Umjetnina", x => x.umjetinaID);
                });

            migrationBuilder.CreateTable(
                name: "KorisnikAukcija",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    aukcijaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikAukcija", x => x.id);
                    table.ForeignKey(
                        name: "FK_KorisnikAukcija_Aukcija_aukcijaID",
                        column: x => x.aukcijaID,
                        principalTable: "Aukcija",
                        principalColumn: "AukcijaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KorisnikAukcija_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifikacijaKorisnik",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    notifikacijaID = table.Column<int>(type: "int", nullable: false),
                    KorisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifikacijaKorisnik", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotifikacijaKorisnik_Korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifikacijaKorisnik_Notifikacija_notifikacijaID",
                        column: x => x.notifikacijaID,
                        principalTable: "Notifikacija",
                        principalColumn: "notifikacijaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifikacijaUmjetnina",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    notifikacijaID = table.Column<int>(type: "int", nullable: false),
                    umjetninaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifikacijaUmjetnina", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotifikacijaUmjetnina_Notifikacija_notifikacijaID",
                        column: x => x.notifikacijaID,
                        principalTable: "Notifikacija",
                        principalColumn: "notifikacijaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifikacijaUmjetnina_Umjetnina_umjetninaID",
                        column: x => x.umjetninaID,
                        principalTable: "Umjetnina",
                        principalColumn: "umjetinaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikAukcija_aukcijaID",
                table: "KorisnikAukcija",
                column: "aukcijaID");

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikAukcija_korisnikID",
                table: "KorisnikAukcija",
                column: "korisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijaKorisnik_KorisnikID",
                table: "NotifikacijaKorisnik",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijaKorisnik_notifikacijaID",
                table: "NotifikacijaKorisnik",
                column: "notifikacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijaUmjetnina_notifikacijaID",
                table: "NotifikacijaUmjetnina",
                column: "notifikacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijaUmjetnina_umjetninaID",
                table: "NotifikacijaUmjetnina",
                column: "umjetninaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KorisnikAukcija");

            migrationBuilder.DropTable(
                name: "NotifikacijaKorisnik");

            migrationBuilder.DropTable(
                name: "NotifikacijaUmjetnina");

            migrationBuilder.DropTable(
                name: "Aukcija");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Notifikacija");

            migrationBuilder.DropTable(
                name: "Umjetnina");
        }
    }
}
