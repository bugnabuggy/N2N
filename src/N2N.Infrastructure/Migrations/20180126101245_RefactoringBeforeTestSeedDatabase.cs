using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace N2N.Infrastructure.Migrations
{
    public partial class RefactoringBeforeTestSeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PromisesToUsers",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    N2NUserId = table.Column<Guid>(nullable: false),
                    PostalCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_N2NUsers_N2NUserId",
                        column: x => x.N2NUserId,
                        principalTable: "N2NUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Postcards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    N2NUserId = table.Column<Guid>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Postcards_N2NUsers_N2NUserId",
                        column: x => x.N2NUserId,
                        principalTable: "N2NUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddresseses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<int>(nullable: false),
                    N2NUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresseses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresseses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAddresseses_N2NUsers_N2NUserId",
                        column: x => x.N2NUserId,
                        principalTable: "N2NUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostcardAddresseses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<int>(nullable: false),
                    PostcardId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostcardAddresseses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostcardAddresseses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostcardAddresseses_Postcards_PostcardId",
                        column: x => x.PostcardId,
                        principalTable: "Postcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_N2NUserId",
                table: "Addresses",
                column: "N2NUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostcardAddresseses_AddressId",
                table: "PostcardAddresseses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PostcardAddresseses_PostcardId",
                table: "PostcardAddresseses",
                column: "PostcardId");

            migrationBuilder.CreateIndex(
                name: "IX_Postcards_N2NUserId",
                table: "Postcards",
                column: "N2NUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresseses_AddressId",
                table: "UserAddresseses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresseses_N2NUserId",
                table: "UserAddresseses",
                column: "N2NUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostcardAddresseses");

            migrationBuilder.DropTable(
                name: "UserAddresseses");

            migrationBuilder.DropTable(
                name: "Postcards");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PromisesToUsers",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
