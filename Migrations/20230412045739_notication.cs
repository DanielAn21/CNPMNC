using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibManager.Migrations
{
    public partial class notication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notications",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    senderid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    receiverid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    typeMessage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notications", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notications_Users_receiverid",
                        column: x => x.receiverid,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Notications_Users_senderid",
                        column: x => x.senderid,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notications_receiverid",
                table: "Notications",
                column: "receiverid");

            migrationBuilder.CreateIndex(
                name: "IX_Notications_senderid",
                table: "Notications",
                column: "senderid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notications");
        }
    }
}
