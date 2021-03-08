using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class FixKeyForCategoryAndProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Products_ProductId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProductId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "67fde54a-001d-4125-bd05-2fcccd082126");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "822b3bd3-99c9-4a4b-81d5-9b9303351608", "AQAAAAEAACcQAAAAEJNFqEsjMmFUWdBzSBhX0lBwOY4CoWE6An8Gqna6l0wbhTi0tjBs3COggoOByjMB8Q==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 3, 7, 15, 39, 41, 626, DateTimeKind.Local).AddTicks(8921));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "49171c0e-94ca-4088-aca9-22b623805d3b");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9a9ab1ca-4733-461e-abce-272f69b798de", "AQAAAAEAACcQAAAAELT2HKgjbzFJ6Epwx/pFA2m3avmBhvMGMTV39fh1Oa6wAdmSwd/6oCzJhfgF+p6wkQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 3, 6, 15, 16, 20, 749, DateTimeKind.Local).AddTicks(6272));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProductId",
                table: "Categories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Products_ProductId",
                table: "Categories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
