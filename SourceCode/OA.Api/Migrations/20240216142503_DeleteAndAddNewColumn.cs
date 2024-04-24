using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OA.Api.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAndAddNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFile2",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ImageFile3",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ImageFile4",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "ImageFile1",
                table: "Item",
                newName: "ImageFile");

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "Item",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Item",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "ImageFile",
                table: "Item",
                newName: "ImageFile1");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageFile2",
                table: "Item",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageFile3",
                table: "Item",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageFile4",
                table: "Item",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
