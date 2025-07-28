using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClankyWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clanky : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clanky",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Titulek = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Perex = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clanky", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clanky_UrlKod",
                table: "Clanky",
                column: "UrlKod",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clanky");
        }
    }
}
