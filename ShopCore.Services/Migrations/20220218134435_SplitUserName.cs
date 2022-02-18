namespace ShopCore.Services.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SplitUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Account",
                table: "OrderDetails",
                newName: "TypeLogin");

            migrationBuilder.RenameColumn(
                name: "Account",
                table: "Carts",
                newName: "TypeLogin");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "TypeLogin",
                table: "OrderDetails",
                newName: "Account");

            migrationBuilder.RenameColumn(
                name: "TypeLogin",
                table: "Carts",
                newName: "Account");
        }
    }
}
