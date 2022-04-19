using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SamuraiApp.Data.Migrations
{
    public partial class personfullnamecolumnnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BetterName_SurName",
                table: "Samurais",
                newName: "SurName");

            migrationBuilder.RenameColumn(
                name: "BetterName_GivenName",
                table: "Samurais",
                newName: "GivenName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "Samurais",
                newName: "BetterName_SurName");

            migrationBuilder.RenameColumn(
                name: "GivenName",
                table: "Samurais",
                newName: "BetterName_GivenName");
        }
    }
}
