using Microsoft.EntityFrameworkCore.Migrations;

namespace CashManager.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NbOfWronCheques",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "BankAccounts");

            migrationBuilder.AddColumn<int>(
                name: "NbOfWrongCheques",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price", "Reference" },
                values: new object[] { 1, "Produit de test", 10f, "00000001" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "NbOfWrongCheques", "NnOfWrongCards", "Password", "Username" },
                values: new object[] { 1, 0, 0, "Password1", "Username1" });

            migrationBuilder.InsertData(
                table: "BankAccounts",
                columns: new[] { "Id", "Balance", "OwnerId" },
                values: new object[] { 1, 10000f, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "NbOfWrongCheques",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "NbOfWronCheques",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "BankAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
