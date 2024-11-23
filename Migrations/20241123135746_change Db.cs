using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropSpace.Migrations
{
    /// <inheritdoc />
    public partial class changeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Divisions_divisionId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Countries_countryId",
                table: "Divisions");

            migrationBuilder.DropForeignKey(
                name: "FK_personsDatas_UnionWards_unionId",
                table: "personsDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_personsDatas_Villages_villageId",
                table: "personsDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_Thanas_Districts_districtId",
                table: "Thanas");

            migrationBuilder.DropForeignKey(
                name: "FK_Thanas_RangeMetros_rangeMetroId",
                table: "Thanas");

            migrationBuilder.DropForeignKey(
                name: "FK_UnionWards_Districts_districtsId",
                table: "UnionWards");

            migrationBuilder.DropForeignKey(
                name: "FK_UnionWards_Thanas_thanaId",
                table: "UnionWards");

            migrationBuilder.DropForeignKey(
                name: "FK_uploadedFiles_personsDatas_personsDataId",
                table: "uploadedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Villages_Districts_districtsId",
                table: "Villages");

            migrationBuilder.DropForeignKey(
                name: "FK_Villages_Thanas_thanaId",
                table: "Villages");

            migrationBuilder.DropForeignKey(
                name: "FK_Villages_UnionWards_unionWardId",
                table: "Villages");

            migrationBuilder.DropTable(
                name: "postOffices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Villages",
                table: "Villages");

            migrationBuilder.DropIndex(
                name: "IX_Villages_districtsId",
                table: "Villages");

            migrationBuilder.DropIndex(
                name: "IX_Villages_thanaId",
                table: "Villages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnionWards",
                table: "UnionWards");

            migrationBuilder.DropIndex(
                name: "IX_UnionWards_districtsId",
                table: "UnionWards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Thanas",
                table: "Thanas");

            migrationBuilder.DropIndex(
                name: "IX_Thanas_rangeMetroId",
                table: "Thanas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RangeMetros",
                table: "RangeMetros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Districts",
                table: "Districts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personsDatas",
                table: "personsDatas");

            migrationBuilder.DropColumn(
                name: "districtsId",
                table: "Villages");

            migrationBuilder.DropColumn(
                name: "thanaId",
                table: "Villages");

            migrationBuilder.DropColumn(
                name: "districtsId",
                table: "UnionWards");

            migrationBuilder.DropColumn(
                name: "rangeMetroId",
                table: "Thanas");

            migrationBuilder.DropColumn(
                name: "pimsRangeId",
                table: "RangeMetros");

            migrationBuilder.DropColumn(
                name: "pimsRangeName",
                table: "RangeMetros");

            migrationBuilder.DropColumn(
                name: "nationality",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "bpNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "otpCode",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Villages",
                newName: "villages");

            migrationBuilder.RenameTable(
                name: "UnionWards",
                newName: "unionWards");

            migrationBuilder.RenameTable(
                name: "Thanas",
                newName: "thanas");

            migrationBuilder.RenameTable(
                name: "RangeMetros",
                newName: "rangeMetros");

            migrationBuilder.RenameTable(
                name: "Divisions",
                newName: "divisions");

            migrationBuilder.RenameTable(
                name: "Districts",
                newName: "districts");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "countries");

            migrationBuilder.RenameTable(
                name: "personsDatas",
                newName: "personalDatas");

            migrationBuilder.RenameIndex(
                name: "IX_Villages_unionWardId",
                table: "villages",
                newName: "IX_villages_unionWardId");

            migrationBuilder.RenameIndex(
                name: "IX_UnionWards_thanaId",
                table: "unionWards",
                newName: "IX_unionWards_thanaId");

            migrationBuilder.RenameIndex(
                name: "IX_Thanas_districtId",
                table: "thanas",
                newName: "IX_thanas_districtId");

            migrationBuilder.RenameIndex(
                name: "IX_Divisions_countryId",
                table: "divisions",
                newName: "IX_divisions_countryId");

            migrationBuilder.RenameIndex(
                name: "IX_Districts_divisionId",
                table: "districts",
                newName: "IX_districts_divisionId");

            migrationBuilder.RenameIndex(
                name: "IX_personsDatas_villageId",
                table: "personalDatas",
                newName: "IX_personalDatas_villageId");

            migrationBuilder.RenameIndex(
                name: "IX_personsDatas_unionId",
                table: "personalDatas",
                newName: "IX_personalDatas_unionId");

            migrationBuilder.AddColumn<int>(
                name: "crimeTypeId",
                table: "uploadedFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "districtId",
                table: "thanas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "countryId",
                table: "divisions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updatedBy",
                table: "AspNetUsers",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userType",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_villages",
                table: "villages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_unionWards",
                table: "unionWards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_thanas",
                table: "thanas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rangeMetros",
                table: "rangeMetros",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_divisions",
                table: "divisions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_districts",
                table: "districts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_countries",
                table: "countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_personalDatas",
                table: "personalDatas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "crimeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    crimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crimeInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "divisionDistricts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rangeMetroId = table.Column<int>(type: "int", nullable: true),
                    divisionDistrictName = table.Column<string>(type: "NVARCHAR(350)", nullable: true),
                    divisionDistrictNameBn = table.Column<string>(type: "NVARCHAR(350)", nullable: true),
                    isActive = table.Column<string>(type: "NVARCHAR(10)", nullable: true),
                    latitude = table.Column<string>(type: "NVARCHAR(120)", nullable: true),
                    longitude = table.Column<string>(type: "NVARCHAR(120)", nullable: true),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_divisionDistricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_divisionDistricts_rangeMetros_rangeMetroId",
                        column: x => x.rangeMetroId,
                        principalTable: "rangeMetros",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "oTPLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otpExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isVerified = table.Column<bool>(type: "bit", nullable: false),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oTPLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: true),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "zoneCircles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    divisionDistrictId = table.Column<int>(type: "int", nullable: true),
                    zoneName = table.Column<string>(type: "NVARCHAR(350)", nullable: true),
                    zoneNameBn = table.Column<string>(type: "NVARCHAR(350)", nullable: true),
                    isActive = table.Column<string>(type: "NVARCHAR(10)", nullable: true),
                    latitude = table.Column<string>(type: "NVARCHAR(120)", nullable: true),
                    longitude = table.Column<string>(type: "NVARCHAR(120)", nullable: true),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zoneCircles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_zoneCircles_divisionDistricts_divisionDistrictId",
                        column: x => x.divisionDistrictId,
                        principalTable: "divisionDistricts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "policeThanas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rangeMetroId = table.Column<int>(type: "int", nullable: true),
                    divisionDistrictId = table.Column<int>(type: "int", nullable: true),
                    zoneCircleId = table.Column<int>(type: "int", nullable: true),
                    upazillaId = table.Column<int>(type: "int", nullable: true),
                    policeThanaName = table.Column<string>(type: "NVARCHAR(350)", nullable: true),
                    policeThanaNameBn = table.Column<string>(type: "NVARCHAR(350)", nullable: true),
                    isActive = table.Column<string>(type: "NVARCHAR(10)", nullable: true),
                    isReportable = table.Column<string>(type: "NVARCHAR(10)", nullable: true),
                    latitude = table.Column<string>(type: "NVARCHAR(120)", nullable: true),
                    longitude = table.Column<string>(type: "NVARCHAR(120)", nullable: true),
                    policeThanaId = table.Column<int>(type: "int", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fariType = table.Column<int>(type: "int", nullable: true),
                    isChild = table.Column<int>(type: "int", nullable: true),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_policeThanas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_policeThanas_divisionDistricts_divisionDistrictId",
                        column: x => x.divisionDistrictId,
                        principalTable: "divisionDistricts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_policeThanas_policeThanas_policeThanaId",
                        column: x => x.policeThanaId,
                        principalTable: "policeThanas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_policeThanas_rangeMetros_rangeMetroId",
                        column: x => x.rangeMetroId,
                        principalTable: "rangeMetros",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_policeThanas_thanas_upazillaId",
                        column: x => x.upazillaId,
                        principalTable: "thanas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_policeThanas_zoneCircles_zoneCircleId",
                        column: x => x.zoneCircleId,
                        principalTable: "zoneCircles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_uploadedFiles_crimeTypeId",
                table: "uploadedFiles",
                column: "crimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_divisionDistricts_rangeMetroId",
                table: "divisionDistricts",
                column: "rangeMetroId");

            migrationBuilder.CreateIndex(
                name: "IX_policeThanas_divisionDistrictId",
                table: "policeThanas",
                column: "divisionDistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_policeThanas_policeThanaId",
                table: "policeThanas",
                column: "policeThanaId");

            migrationBuilder.CreateIndex(
                name: "IX_policeThanas_rangeMetroId",
                table: "policeThanas",
                column: "rangeMetroId");

            migrationBuilder.CreateIndex(
                name: "IX_policeThanas_upazillaId",
                table: "policeThanas",
                column: "upazillaId");

            migrationBuilder.CreateIndex(
                name: "IX_policeThanas_zoneCircleId",
                table: "policeThanas",
                column: "zoneCircleId");

            migrationBuilder.CreateIndex(
                name: "IX_zoneCircles_divisionDistrictId",
                table: "zoneCircles",
                column: "divisionDistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_districts_divisions_divisionId",
                table: "districts",
                column: "divisionId",
                principalTable: "divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_divisions_countries_countryId",
                table: "divisions",
                column: "countryId",
                principalTable: "countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_personalDatas_unionWards_unionId",
                table: "personalDatas",
                column: "unionId",
                principalTable: "unionWards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_personalDatas_villages_villageId",
                table: "personalDatas",
                column: "villageId",
                principalTable: "villages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_thanas_districts_districtId",
                table: "thanas",
                column: "districtId",
                principalTable: "districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_unionWards_thanas_thanaId",
                table: "unionWards",
                column: "thanaId",
                principalTable: "thanas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploadedFiles_crimeInfos_crimeTypeId",
                table: "uploadedFiles",
                column: "crimeTypeId",
                principalTable: "crimeInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_uploadedFiles_personalDatas_personsDataId",
                table: "uploadedFiles",
                column: "personsDataId",
                principalTable: "personalDatas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_villages_unionWards_unionWardId",
                table: "villages",
                column: "unionWardId",
                principalTable: "unionWards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_districts_divisions_divisionId",
                table: "districts");

            migrationBuilder.DropForeignKey(
                name: "FK_divisions_countries_countryId",
                table: "divisions");

            migrationBuilder.DropForeignKey(
                name: "FK_personalDatas_unionWards_unionId",
                table: "personalDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_personalDatas_villages_villageId",
                table: "personalDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_thanas_districts_districtId",
                table: "thanas");

            migrationBuilder.DropForeignKey(
                name: "FK_unionWards_thanas_thanaId",
                table: "unionWards");

            migrationBuilder.DropForeignKey(
                name: "FK_uploadedFiles_crimeInfos_crimeTypeId",
                table: "uploadedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_uploadedFiles_personalDatas_personsDataId",
                table: "uploadedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_villages_unionWards_unionWardId",
                table: "villages");

            migrationBuilder.DropTable(
                name: "crimeInfos");

            migrationBuilder.DropTable(
                name: "oTPLogs");

            migrationBuilder.DropTable(
                name: "policeThanas");

            migrationBuilder.DropTable(
                name: "userTypes");

            migrationBuilder.DropTable(
                name: "zoneCircles");

            migrationBuilder.DropTable(
                name: "divisionDistricts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_villages",
                table: "villages");

            migrationBuilder.DropIndex(
                name: "IX_uploadedFiles_crimeTypeId",
                table: "uploadedFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_unionWards",
                table: "unionWards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_thanas",
                table: "thanas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rangeMetros",
                table: "rangeMetros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_divisions",
                table: "divisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_districts",
                table: "districts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_countries",
                table: "countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personalDatas",
                table: "personalDatas");

            migrationBuilder.DropColumn(
                name: "crimeTypeId",
                table: "uploadedFiles");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "updatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userType",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "villages",
                newName: "Villages");

            migrationBuilder.RenameTable(
                name: "unionWards",
                newName: "UnionWards");

            migrationBuilder.RenameTable(
                name: "thanas",
                newName: "Thanas");

            migrationBuilder.RenameTable(
                name: "rangeMetros",
                newName: "RangeMetros");

            migrationBuilder.RenameTable(
                name: "divisions",
                newName: "Divisions");

            migrationBuilder.RenameTable(
                name: "districts",
                newName: "Districts");

            migrationBuilder.RenameTable(
                name: "countries",
                newName: "Countries");

            migrationBuilder.RenameTable(
                name: "personalDatas",
                newName: "personsDatas");

            migrationBuilder.RenameIndex(
                name: "IX_villages_unionWardId",
                table: "Villages",
                newName: "IX_Villages_unionWardId");

            migrationBuilder.RenameIndex(
                name: "IX_unionWards_thanaId",
                table: "UnionWards",
                newName: "IX_UnionWards_thanaId");

            migrationBuilder.RenameIndex(
                name: "IX_thanas_districtId",
                table: "Thanas",
                newName: "IX_Thanas_districtId");

            migrationBuilder.RenameIndex(
                name: "IX_divisions_countryId",
                table: "Divisions",
                newName: "IX_Divisions_countryId");

            migrationBuilder.RenameIndex(
                name: "IX_districts_divisionId",
                table: "Districts",
                newName: "IX_Districts_divisionId");

            migrationBuilder.RenameIndex(
                name: "IX_personalDatas_villageId",
                table: "personsDatas",
                newName: "IX_personsDatas_villageId");

            migrationBuilder.RenameIndex(
                name: "IX_personalDatas_unionId",
                table: "personsDatas",
                newName: "IX_personsDatas_unionId");

            migrationBuilder.AddColumn<int>(
                name: "districtsId",
                table: "Villages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "thanaId",
                table: "Villages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "districtsId",
                table: "UnionWards",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "districtId",
                table: "Thanas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "rangeMetroId",
                table: "Thanas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "pimsRangeId",
                table: "RangeMetros",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pimsRangeName",
                table: "RangeMetros",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "countryId",
                table: "Divisions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "nationality",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bpNo",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otpCode",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Villages",
                table: "Villages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnionWards",
                table: "UnionWards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Thanas",
                table: "Thanas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RangeMetros",
                table: "RangeMetros",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Districts",
                table: "Districts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_personsDatas",
                table: "personsDatas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "postOffices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thanaId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    isDelete = table.Column<int>(type: "int", nullable: true),
                    postalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    postalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    postalNameBn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    postalShortName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedBy = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_postOffices_Thanas_thanaId",
                        column: x => x.thanaId,
                        principalTable: "Thanas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Villages_districtsId",
                table: "Villages",
                column: "districtsId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_thanaId",
                table: "Villages",
                column: "thanaId");

            migrationBuilder.CreateIndex(
                name: "IX_UnionWards_districtsId",
                table: "UnionWards",
                column: "districtsId");

            migrationBuilder.CreateIndex(
                name: "IX_Thanas_rangeMetroId",
                table: "Thanas",
                column: "rangeMetroId");

            migrationBuilder.CreateIndex(
                name: "IX_postOffices_thanaId",
                table: "postOffices",
                column: "thanaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Divisions_divisionId",
                table: "Districts",
                column: "divisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Countries_countryId",
                table: "Divisions",
                column: "countryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_personsDatas_UnionWards_unionId",
                table: "personsDatas",
                column: "unionId",
                principalTable: "UnionWards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_personsDatas_Villages_villageId",
                table: "personsDatas",
                column: "villageId",
                principalTable: "Villages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Thanas_Districts_districtId",
                table: "Thanas",
                column: "districtId",
                principalTable: "Districts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Thanas_RangeMetros_rangeMetroId",
                table: "Thanas",
                column: "rangeMetroId",
                principalTable: "RangeMetros",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnionWards_Districts_districtsId",
                table: "UnionWards",
                column: "districtsId",
                principalTable: "Districts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnionWards_Thanas_thanaId",
                table: "UnionWards",
                column: "thanaId",
                principalTable: "Thanas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_uploadedFiles_personsDatas_personsDataId",
                table: "uploadedFiles",
                column: "personsDataId",
                principalTable: "personsDatas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Villages_Districts_districtsId",
                table: "Villages",
                column: "districtsId",
                principalTable: "Districts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Villages_Thanas_thanaId",
                table: "Villages",
                column: "thanaId",
                principalTable: "Thanas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Villages_UnionWards_unionWardId",
                table: "Villages",
                column: "unionWardId",
                principalTable: "UnionWards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
