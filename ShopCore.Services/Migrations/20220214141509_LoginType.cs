namespace ShopCore.Services.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class LoginType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginType",
                table: "Users");
        }
    }
}
