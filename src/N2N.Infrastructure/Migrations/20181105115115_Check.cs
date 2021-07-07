using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace N2N.Infrastructure.Migrations
{
    public partial class Check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdRefreshToken",
                table: "N2NTokens",
                newName: "RefreshTokenId");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpirationDate",
                table: "N2NRefreshTokens",
                newName: "TokenExpirationDate");

            migrationBuilder.AddColumn<string>(
                name: "HashIdLinkPromise",
                table: "Promises",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashIdLinkPromise",
                table: "Promises");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenId",
                table: "N2NTokens",
                newName: "IdRefreshToken");

            migrationBuilder.RenameColumn(
                name: "TokenExpirationDate",
                table: "N2NRefreshTokens",
                newName: "RefreshTokenExpirationDate");
        }
    }
}
