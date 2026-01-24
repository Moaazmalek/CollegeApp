using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingStudentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DOB", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "India", new DateTime(2000, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Venkat@gmail.com", "Venkat" },
                    { 2, "Jordan", new DateTime(2003, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "muath@gmail.com", "Muath" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
