using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASMNhom3.Migrations
{
    public partial class acoount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Carts_BookID",
                table: "Carts",
                column: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Books_BookID",
                table: "Carts",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Books_BookID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_BookID",
                table: "Carts");
        }
    }
}
