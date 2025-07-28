using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClankyWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Komentare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Komentare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClanekId = table.Column<int>(type: "int", nullable: false),
                    VlozilId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentare", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komentare_AspNetUsers_VlozilId",
                        column: x => x.VlozilId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Komentare_Clanky_ClanekId",
                        column: x => x.ClanekId,
                        principalTable: "Clanky",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Komentare_ClanekId",
                table: "Komentare",
                column: "ClanekId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentare_VlozilId",
                table: "Komentare",
                column: "VlozilId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Komentare");
        }
    }
}
