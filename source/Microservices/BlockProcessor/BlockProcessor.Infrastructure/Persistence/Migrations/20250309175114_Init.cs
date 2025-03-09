using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockProcessor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockProgress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockNumber = table.Column<long>(type: "bigint", nullable: false),
                    Chain = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RpcUrl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Chain = table.Column<int>(type: "int", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlockOfConfirmation = table.Column<int>(type: "int", nullable: false),
                    WaitIntervalOfBlockProgress = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RpcUrl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transfer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Chain = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(38,19)", nullable: false),
                    GasUsed = table.Column<decimal>(type: "decimal(38,19)", nullable: false),
                    EffectiveGasPrice = table.Column<decimal>(type: "decimal(38,19)", nullable: false),
                    CumulativeGasUsed = table.Column<decimal>(type: "decimal(38,19)", nullable: false),
                    BlockNumber = table.Column<long>(type: "bigint", nullable: false),
                    ConfirmedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletAddress",
                columns: table => new
                {
                    Address = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletAddress", x => x.Address);
                });

            migrationBuilder.CreateTable(
                name: "Erc20Transfer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(38,19)", nullable: false),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erc20Transfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Erc20Transfer_Transfer_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Erc721Transfer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenId = table.Column<long>(type: "bigint", nullable: false),
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erc721Transfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Erc721Transfer_Transfer_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockProgress_Chain_BlockNumber",
                table: "BlockProgress",
                columns: new[] { "Chain", "BlockNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Erc20Transfer_TransferId",
                table: "Erc20Transfer",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Erc721Transfer_TransferId",
                table: "Erc721Transfer",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_BlockNumber",
                table: "Transfer",
                column: "BlockNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_Hash_Chain",
                table: "Transfer",
                columns: new[] { "Hash", "Chain" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletAddress_Address",
                table: "WalletAddress",
                column: "Address",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockProgress");

            migrationBuilder.DropTable(
                name: "Erc20Transfer");

            migrationBuilder.DropTable(
                name: "Erc721Transfer");

            migrationBuilder.DropTable(
                name: "RpcUrl");

            migrationBuilder.DropTable(
                name: "WalletAddress");

            migrationBuilder.DropTable(
                name: "Transfer");
        }
    }
}
