using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetoAPI.Migrations
{
    public partial class AddRoleAndProductUserRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "id_usuario",
                table: "produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_produtos_id_usuario",
                table: "produtos",
                column: "id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_produtos_usuarios_id_usuario",
                table: "produtos",
                column: "id_usuario",
                principalTable: "usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produtos_usuarios_id_usuario",
                table: "produtos");

            migrationBuilder.DropIndex(
                name: "IX_produtos_id_usuario",
                table: "produtos");

            migrationBuilder.DropColumn(
                name: "role",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "id_usuario",
                table: "produtos");
        }
    }
}
