using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropSpace.Migrations
{
    /// <inheritdoc />
    public partial class AddedFiletypeFileLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isWhiteList",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "fileTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fileTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fileTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fileLimits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fileTypeId = table.Column<int>(type: "int", nullable: false),
                    hourFileNo = table.Column<int>(type: "int", nullable: false),
                    hourFileSize = table.Column<int>(type: "int", nullable: false),
                    dayFileNo = table.Column<int>(type: "int", nullable: false),
                    dayFileSize = table.Column<int>(type: "int", nullable: false),
                    totalFileNo = table.Column<int>(type: "int", nullable: false),
                    totalFileSize = table.Column<int>(type: "int", nullable: false),
                    alertFileSize = table.Column<int>(type: "int", nullable: false),
                    archiveFileSize = table.Column<int>(type: "int", nullable: false),
                    archivingMonth = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fileLimits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fileLimits_fileTypes_fileTypeId",
                        column: x => x.fileTypeId,
                        principalTable: "fileTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fileLimits_fileTypeId",
                table: "fileLimits",
                column: "fileTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fileLimits");

            migrationBuilder.DropTable(
                name: "fileTypes");

            migrationBuilder.DropColumn(
                name: "isWhiteList",
                table: "AspNetUsers");
        }
    }
}
