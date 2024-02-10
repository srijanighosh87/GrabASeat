using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Grab.A.Seat.Domains.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactNumber",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "ContactNumber", "CreationDate", "Email", "Name" },
                values: new object[,]
                {
                    { new Guid("3dd83670-a736-48fc-838f-bceab7a30735"), "9876541232", new DateTime(2024, 2, 10, 21, 13, 48, 861, DateTimeKind.Utc).AddTicks(9948), "srijanighosh87@gmail.com", "Srijani" },
                    { new Guid("54f3537a-7724-4fad-bb5e-348cc9a4d19d"), "1234567895", new DateTime(2024, 2, 10, 21, 13, 48, 861, DateTimeKind.Utc).AddTicks(9951), "sricheta1994@gmail.com", "Sricheta" }
                });

            migrationBuilder.InsertData(
                table: "Sequences",
                columns: new[] { "Id", "CreationDate", "CuurentValue", "IncreaseBy", "SequenceName", "Start" },
                values: new object[] { new Guid("d588116a-435d-4c05-a40d-8db97ae4263a"), new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(52), 1.0, 1.0, "Booking_Reference", 1.0 });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "Capacity", "CreationDate", "TableNumber" },
                values: new object[,]
                {
                    { new Guid("220105b1-487d-4e9d-b2df-21ce54d30051"), 2, new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(30), "Table01" },
                    { new Guid("4e221cd3-1111-44e8-849f-1ab4770dd17e"), 2, new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(35), "Table03" },
                    { new Guid("74981a7f-8441-48ab-a10c-5e58f368f4f6"), 3, new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(37), "Table04" },
                    { new Guid("afd7fea7-3bfb-4231-99db-02bbd19304fb"), 5, new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(33), "Table02" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingEndDateTime", "BookingReference", "BookingStartDateTime", "Comments", "CreationDate", "Customer_Id", "PartySize", "Table_Id" },
                values: new object[,]
                {
                    { new Guid("7356f531-3b5e-4b24-bf43-c72604bae1b3"), new DateTime(2024, 2, 10, 23, 13, 48, 862, DateTimeKind.Utc).AddTicks(85), "0003", new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(85), "", new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(86), new Guid("54f3537a-7724-4fad-bb5e-348cc9a4d19d"), 2, new Guid("4e221cd3-1111-44e8-849f-1ab4770dd17e") },
                    { new Guid("88c8351d-ccd8-467e-bd47-5fb326c67726"), new DateTime(2024, 2, 10, 23, 13, 48, 862, DateTimeKind.Utc).AddTicks(74), "0001", new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(73), "Non Veg Only", new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(78), new Guid("3dd83670-a736-48fc-838f-bceab7a30735"), 2, new Guid("220105b1-487d-4e9d-b2df-21ce54d30051") },
                    { new Guid("bf3992a2-a8f8-4323-ac37-89ab7f4ad01a"), new DateTime(2024, 2, 10, 23, 13, 48, 862, DateTimeKind.Utc).AddTicks(81), "0002", new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(81), "Window seat", new DateTime(2024, 2, 10, 21, 13, 48, 862, DateTimeKind.Utc).AddTicks(82), new Guid("54f3537a-7724-4fad-bb5e-348cc9a4d19d"), 2, new Guid("afd7fea7-3bfb-4231-99db-02bbd19304fb") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name_ContactNumber",
                table: "Customers",
                columns: new[] { "Name", "ContactNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_Name_ContactNumber",
                table: "Customers");

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7356f531-3b5e-4b24-bf43-c72604bae1b3"));

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("88c8351d-ccd8-467e-bd47-5fb326c67726"));

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("bf3992a2-a8f8-4323-ac37-89ab7f4ad01a"));

            migrationBuilder.DeleteData(
                table: "Sequences",
                keyColumn: "Id",
                keyValue: new Guid("d588116a-435d-4c05-a40d-8db97ae4263a"));

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: new Guid("74981a7f-8441-48ab-a10c-5e58f368f4f6"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("3dd83670-a736-48fc-838f-bceab7a30735"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("54f3537a-7724-4fad-bb5e-348cc9a4d19d"));

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: new Guid("220105b1-487d-4e9d-b2df-21ce54d30051"));

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: new Guid("4e221cd3-1111-44e8-849f-1ab4770dd17e"));

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: new Guid("afd7fea7-3bfb-4231-99db-02bbd19304fb"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactNumber",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
