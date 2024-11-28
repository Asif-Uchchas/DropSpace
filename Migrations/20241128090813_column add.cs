using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropSpace.Migrations
{
    /// <inheritdoc />
    public partial class columnadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isWhiteList",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "personalDatas",
                newName: "crimeTime");

            migrationBuilder.AddColumn<string>(
                name: "shortUrl",
                table: "uploadedFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "crimeDate",
                table: "personalDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "crimeName",
                table: "personalDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "crimePlace",
                table: "personalDatas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shortUrl",
                table: "uploadedFiles");

            migrationBuilder.DropColumn(
                name: "crimeDate",
                table: "personalDatas");

            migrationBuilder.DropColumn(
                name: "crimeName",
                table: "personalDatas");

            migrationBuilder.DropColumn(
                name: "crimePlace",
                table: "personalDatas");

            migrationBuilder.RenameColumn(
                name: "crimeTime",
                table: "personalDatas",
                newName: "name");

            migrationBuilder.AddColumn<bool>(
                name: "isWhiteList",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
