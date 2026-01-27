using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDepartmentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "DepartmentDescription",
                value: "ECE Department");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "DepartmentDescription",
                value: "CSE Department");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "DepartmentDescription",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "DepartmentDescription",
                value: null);
        }
    }
}
