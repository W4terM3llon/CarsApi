using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIS_project.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    DbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Since = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.DbId);
                });

            migrationBuilder.CreateTable(
                name: "Engine",
                columns: table => new
                {
                    DbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerDbId = table.Column<int>(type: "int", nullable: false),
                    HP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engine", x => x.DbId);
                    table.ForeignKey(
                        name: "FK_Engine_Manufacturer_ManufacturerDbId",
                        column: x => x.ManufacturerDbId,
                        principalTable: "Manufacturer",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    DbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerDbId = table.Column<int>(type: "int", nullable: true),
                    EngineDbId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.DbId);
                    table.ForeignKey(
                        name: "FK_Car_Engine_EngineDbId",
                        column: x => x.EngineDbId,
                        principalTable: "Engine",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Car_Manufacturer_ManufacturerDbId",
                        column: x => x.ManufacturerDbId,
                        principalTable: "Manufacturer",
                        principalColumn: "DbId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_EngineDbId",
                table: "Car",
                column: "EngineDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_ManufacturerDbId",
                table: "Car",
                column: "ManufacturerDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Engine_ManufacturerDbId",
                table: "Engine",
                column: "ManufacturerDbId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Engine");

            migrationBuilder.DropTable(
                name: "Manufacturer");
        }
    }
}
