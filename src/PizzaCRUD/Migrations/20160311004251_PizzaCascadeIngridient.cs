using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace PizzaCRUD.Migrations
{
    public partial class PizzaCascadeIngridient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_PizzaIngridient_Pizza_PizzaId", table: "PizzaIngridient");
            migrationBuilder.AddForeignKey(
                name: "FK_PizzaIngridient_Pizza_PizzaId",
                table: "PizzaIngridient",
                column: "PizzaId",
                principalTable: "Pizza",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_PizzaIngridient_Pizza_PizzaId", table: "PizzaIngridient");
            migrationBuilder.AddForeignKey(
                name: "FK_PizzaIngridient_Pizza_PizzaId",
                table: "PizzaIngridient",
                column: "PizzaId",
                principalTable: "Pizza",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
