using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SamuraiApp.Data.Migrations
{
    public partial class personfullname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BetterName_GivenName",
                table: "Samurais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BetterName_SurName",
                table: "Samurais",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetterName_GivenName",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "BetterName_SurName",
                table: "Samurais");
        }
    }
}
