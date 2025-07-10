using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoritos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Vendedores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Favoritos",
                columns: table => new
                {
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favoritos", x => new { x.ClienteId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_Favoritos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favoritos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_ProdutoId",
                table: "Favoritos",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favoritos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Vendedores");
        }
    }
}
