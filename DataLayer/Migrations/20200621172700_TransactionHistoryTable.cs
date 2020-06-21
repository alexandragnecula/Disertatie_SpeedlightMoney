using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class TransactionHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    SenderId = table.Column<long>(nullable: false),
                    BeneficiarId = table.Column<long>(nullable: false),
                    CurrencyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionHistory_User_BeneficiarId",
                        column: x => x.BeneficiarId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionHistory_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionHistory_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_BeneficiarId",
                table: "TransactionHistory",
                column: "BeneficiarId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_CurrencyId",
                table: "TransactionHistory",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_SenderId",
                table: "TransactionHistory",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionHistory");
        }
    }
}
