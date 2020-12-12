using Microsoft.EntityFrameworkCore.Migrations;

namespace CashManager.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NnOfWrongCards",
                table: "Users",
                newName: "NbOfWrongCards");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "NbOfWrongCards", "NbOfWrongCheques", "Password", "Username" },
                values: new object[] { 2, 2, 2, "Password2", "Username2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
