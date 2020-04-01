using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuyKhanhSolution1.Data.Migrations
{
    public partial class initial_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 2, 1, 55, 15, 804, DateTimeKind.Local).AddTicks(3363),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 4, 2, 1, 49, 49, 988, DateTimeKind.Local).AddTicks(9684));

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
                value: new DateTime(2020, 4, 2, 1, 55, 15, 816, DateTimeKind.Local).AddTicks(5587));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 2, 1, 49, 49, 988, DateTimeKind.Local).AddTicks(9684),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 4, 2, 1, 55, 15, 804, DateTimeKind.Local).AddTicks(3363));

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
                value: new DateTime(2020, 4, 2, 1, 49, 50, 1, DateTimeKind.Local).AddTicks(1720));
        }
    }
}
