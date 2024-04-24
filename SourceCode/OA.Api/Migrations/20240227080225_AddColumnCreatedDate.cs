using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OA.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnCreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rating_ItemId",
                table: "Rating");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Bid",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ItemId",
                table: "Rating",
                column: "ItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rating_ItemId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Bid");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ItemId",
                table: "Rating",
                column: "ItemId");
        }
    }
}
