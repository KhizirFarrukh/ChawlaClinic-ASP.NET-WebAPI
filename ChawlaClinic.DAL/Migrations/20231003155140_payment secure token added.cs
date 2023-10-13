using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ChawlaClinic.DAL.Migrations
{
    /// <inheritdoc />
    public partial class paymentsecuretokenadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecureToken",
                table: "Payments",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "TokenID",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecureToken",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TokenID",
                table: "Payments");
        }
    }
}
