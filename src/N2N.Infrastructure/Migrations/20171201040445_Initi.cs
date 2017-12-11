using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace N2N.Infrastructure.Migrations
{
    public partial class Initi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "N2NTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    N2NUserId = table.Column<Guid>(nullable: false),
                    TokenExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_N2NTokens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "N2NTokens");
        }
    }
}
