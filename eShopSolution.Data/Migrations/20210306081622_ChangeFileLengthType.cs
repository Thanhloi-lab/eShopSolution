using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class ChangeFileLengthType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "d7581b7a-ba4f-4ddd-bcfd-4ede5c3c7a75");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7c472670-353b-4d8b-8417-c94ae3ad172f", "AQAAAAEAACcQAAAAEFJhOq0Bjc6oPqXTfajfYTXpo+CPWItbBloZF7QWqkatwf19TdRzZ4zs3DK8deUeRw==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 3, 6, 13, 26, 11, 155, DateTimeKind.Local).AddTicks(7681));
        }
    }
}
