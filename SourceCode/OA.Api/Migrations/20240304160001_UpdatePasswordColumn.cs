using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OA.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm một cột tạm thời với kiểu dữ liệu mới
            migrationBuilder.AddColumn<byte[]>(
                name: "Password_Temp",
                table: "User",
                type: "varbinary(max)",
                nullable: false);

            // Chuyển đổi dữ liệu từ cột cũ sang cột mới
            migrationBuilder.Sql(@"
                UPDATE [User]
                SET Password_Temp = CONVERT(varbinary(max), Password)
            ");

            // Xóa cột cũ
            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            // Đổi tên cột tạm thời thành cột cũ
            migrationBuilder.RenameColumn(
                name: "Password_Temp",
                table: "User",
                newName: "Password");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Thêm một cột tạm thời với kiểu dữ liệu cũ
            migrationBuilder.AddColumn<string>(
               name: "Password_Temp",
               table: "User",
               type: "nvarchar(50)",
               maxLength: 50,
               nullable: false);

            // Chuyển đổi dữ liệu từ cột cũ sang cột mới
            migrationBuilder.Sql(@"
                UPDATE [User]
                SET Password_Temp = CONVERT(nvarchar(50), Password, 1)
            ");

            // Xóa cột cũ
            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            // Đổi tên cột tạm thời thành cột cũ
            migrationBuilder.RenameColumn(
                name: "Password_Temp",
                table: "User",
                newName: "Password");
        }
    }
}
