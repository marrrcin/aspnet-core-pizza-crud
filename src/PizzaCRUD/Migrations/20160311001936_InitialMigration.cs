using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace PizzaCRUD.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingridient",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingridient", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Pizza",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DoughType = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PriceLarge = table.Column<decimal>(nullable: false),
                    PriceMedium = table.Column<decimal>(nullable: false),
                    PriceSmall = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizza", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "PizzaIngridient",
                columns: table => new
                {
                    PizzaId = table.Column<Guid>(nullable: false),
                    IngridientId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaIngridient", x => new { x.PizzaId, x.IngridientId });
                    table.ForeignKey(
                        name: "FK_PizzaIngridient_Ingridient_IngridientId",
                        column: x => x.IngridientId,
                        principalTable: "Ingridient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PizzaIngridient_Pizza_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizza",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateIndex(
                name: "IX_Ingridient_Name",
                table: "Ingridient",
                column: "Name",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_Pizza_Name",
                table: "Pizza",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("PizzaIngridient");
            migrationBuilder.DropTable("Ingridient");
            migrationBuilder.DropTable("Pizza");
        }
    }
}
