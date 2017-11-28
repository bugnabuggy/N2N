using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace N2N.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "N2NUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "N2NUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "N2NUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "N2NUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPic",
                table: "N2NUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "N2NUserId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "N2NUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "N2NUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "N2NUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "N2NUsers");

            migrationBuilder.DropColumn(
                name: "UserPic",
                table: "N2NUsers");

            migrationBuilder.DropColumn(
                name: "N2NUserId",
                table: "AspNetUsers");
        }
    }
}
