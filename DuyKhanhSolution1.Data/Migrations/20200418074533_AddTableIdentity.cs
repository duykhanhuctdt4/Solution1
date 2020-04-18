using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DuyKhanhSolution1.Data.Migrations
{
    public partial class AddTableIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 18, 14, 45, 32, 568, DateTimeKind.Local).AddTicks(2595),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 4, 3, 0, 0, 9, 654, DateTimeKind.Local).AddTicks(4074));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("f8483347-1086-458e-a0e7-5d9351503b1b"),
                column: "ConcurrencyStamp",
                value: "ca4ce130-64e4-432d-97dc-d168b136e172");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("f42202b3-0e97-431c-a703-0f164ee61d9e"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1b212661-438e-42a3-8000-5c8995a82722", "AQAAAAEAACcQAAAAEK4xOoHr5X/FbgmB5ZNxpf+I3CcvX4V28n5M4oArA84W65KWHVf7HQzh+lsOEnj8Wg==" });

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
                value: new DateTime(2020, 4, 18, 14, 45, 32, 584, DateTimeKind.Local).AddTicks(1552));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 4, 3, 0, 0, 9, 654, DateTimeKind.Local).AddTicks(4074),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 4, 18, 14, 45, 32, 568, DateTimeKind.Local).AddTicks(2595));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("f8483347-1086-458e-a0e7-5d9351503b1b"),
                column: "ConcurrencyStamp",
                value: "99c191ef-534b-40c4-b7ac-a61c98e7c562");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("f42202b3-0e97-431c-a703-0f164ee61d9e"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "df51a5db-8816-47bc-98bf-b64d52aaf53e", "AQAAAAEAACcQAAAAEJochTNpapB2guq+tmPpYVDcNEWbTYSIhgEKwfQmjdqEA1dZKyOM5K9hy/KPmfatow==" });

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
                value: new DateTime(2020, 4, 3, 0, 0, 9, 668, DateTimeKind.Local).AddTicks(9862));
        }
    }
}
