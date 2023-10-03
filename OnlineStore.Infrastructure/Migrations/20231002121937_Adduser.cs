using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2756afc0-082c-4c32-865e-de6b890da6f7"), "user2" },
                    { new Guid("50701e88-5668-4e85-b854-a42bf9a95e26"), "user1" },
                    { new Guid("d3e9fd6c-bb7f-4c6f-969f-521637e59337"), "user3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("2756afc0-082c-4c32-865e-de6b890da6f7"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("50701e88-5668-4e85-b854-a42bf9a95e26"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("d3e9fd6c-bb7f-4c6f-969f-521637e59337"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");
        }
    }
}
