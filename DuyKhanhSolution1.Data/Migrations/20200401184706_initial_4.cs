using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuyKhanhSolution1.Data.Migrations
{
    public partial class initial_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 2, 1, 47, 6, 479, DateTimeKind.Local).AddTicks(9085),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 4, 2, 1, 45, 5, 898, DateTimeKind.Local).AddTicks(8702));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreate",
                value: new DateTime(2020, 4, 2, 1, 47, 6, 491, DateTimeKind.Local).AddTicks(6113));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 2, 1, 45, 5, 898, DateTimeKind.Local).AddTicks(8702),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 4, 2, 1, 47, 6, 479, DateTimeKind.Local).AddTicks(9085));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreate",
                value: new DateTime(2020, 4, 2, 1, 45, 5, 910, DateTimeKind.Local).AddTicks(9775));
        }
    }
}
