using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlsetTest.Migrations
{
    /// <inheritdoc />
    public partial class AddingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journals_AspNetUsers_UserId",
                table: "Journals");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_AspNetUsers_UserId",
                table: "Journals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journals_AspNetUsers_UserId",
                table: "Journals");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_AspNetUsers_UserId",
                table: "Journals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
