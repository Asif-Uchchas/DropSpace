using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropSpace.Migrations
{
    /// <inheritdoc />
    public partial class columnadd2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "newFileName",
                table: "uploadedFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "oldFileName",
                table: "uploadedFiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "newFileName",
                table: "uploadedFiles");

            migrationBuilder.DropColumn(
                name: "oldFileName",
                table: "uploadedFiles");
        }
    }
}
