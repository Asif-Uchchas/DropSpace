using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropSpace.Migrations
{
    /// <inheritdoc />
    public partial class tableadd3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "fileSize",
                table: "uploadedFiles",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isWhiteList",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileSize",
                table: "uploadedFiles");

            migrationBuilder.DropColumn(
                name: "isWhiteList",
                table: "AspNetUsers");
        }
    }
}
