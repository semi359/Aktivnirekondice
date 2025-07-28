using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClankyWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class Clanky_Add_AutorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutorId",
                table: "Clanky",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clanky_AutorId",
                table: "Clanky",
                column: "AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clanky_AspNetUsers_AutorId",
                table: "Clanky",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clanky_AspNetUsers_AutorId",
                table: "Clanky");

            migrationBuilder.DropIndex(
                name: "IX_Clanky_AutorId",
                table: "Clanky");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "Clanky");
        }
    }
}
