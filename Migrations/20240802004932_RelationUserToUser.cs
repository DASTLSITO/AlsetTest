using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlsetTest.Migrations
{
    /// <inheritdoc />
    public partial class RelationUserToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSuscriber",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SuscriberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSuscriber", x => new { x.UserId, x.SuscriberId });
                    table.ForeignKey(
                        name: "FK_UserSuscriber_AspNetUsers_SuscriberId",
                        column: x => x.SuscriberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSuscriber_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSuscriber_SuscriberId",
                table: "UserSuscriber",
                column: "SuscriberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSuscriber");
        }
    }
}
