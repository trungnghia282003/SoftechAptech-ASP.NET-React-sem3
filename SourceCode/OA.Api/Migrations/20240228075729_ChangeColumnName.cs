using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OA.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinBidPrice",
                table: "Item",
                newName: "MinIncreasePrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinIncreasePrice",
                table: "Item",
                newName: "MinBidPrice");
        }
    }
}
