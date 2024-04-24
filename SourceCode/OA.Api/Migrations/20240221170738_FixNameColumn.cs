using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OA.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_SellId",
                table: "Rating");

            migrationBuilder.RenameColumn(
                name: "SellId",
                table: "Rating",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_SellId",
                table: "Rating",
                newName: "IX_Rating_SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_SellerId",
                table: "Rating",
                column: "SellerId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_SellerId",
                table: "Rating");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Rating",
                newName: "SellId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_SellerId",
                table: "Rating",
                newName: "IX_Rating_SellId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_SellId",
                table: "Rating",
                column: "SellId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
