using Microsoft.EntityFrameworkCore.Migrations;

namespace Belt.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "wallet",
                table: "Users",
                newName: "Wallet");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "product",
                table: "Auctions",
                newName: "Product");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Auctions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "top_bidder_id",
                table: "Auctions",
                newName: "TopBidderId");

            migrationBuilder.RenameColumn(
                name: "top_bid",
                table: "Auctions",
                newName: "TopBid");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "Auctions",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "creator_id",
                table: "Auctions",
                newName: "CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Wallet",
                table: "Users",
                newName: "wallet");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "Product",
                table: "Auctions",
                newName: "product");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Auctions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "TopBidderId",
                table: "Auctions",
                newName: "top_bidder_id");

            migrationBuilder.RenameColumn(
                name: "TopBid",
                table: "Auctions",
                newName: "top_bid");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Auctions",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Auctions",
                newName: "creator_id");
        }
    }
}
