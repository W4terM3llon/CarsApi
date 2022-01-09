using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIS_project.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owner",
                columns: table => new
                {
                    DbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.DbId);
                });

            migrationBuilder.CreateTable(
                name: "CarOwner",
                columns: table => new
                {
                    CarsDbId = table.Column<int>(type: "int", nullable: false),
                    OwnersDbId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarOwner", x => new { x.CarsDbId, x.OwnersDbId });
                    table.ForeignKey(
                        name: "FK_CarOwner_Car_CarsDbId",
                        column: x => x.CarsDbId,
                        principalTable: "Car",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarOwner_Owner_OwnersDbId",
                        column: x => x.OwnersDbId,
                        principalTable: "Owner",
                        principalColumn: "DbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarOwner_OwnersDbId",
                table: "CarOwner",
                column: "OwnersDbId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarOwner");

            migrationBuilder.DropTable(
                name: "Owner");
        }
    }
}
