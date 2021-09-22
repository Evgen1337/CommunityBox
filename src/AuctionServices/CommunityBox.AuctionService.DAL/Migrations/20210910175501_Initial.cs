using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CommunityBox.AuctionService.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerUserId = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    CreationUtcDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateUtcDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StartingPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auctioneers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Bet = table.Column<decimal>(type: "numeric", nullable: false),
                    AuctionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctioneers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auctioneers_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    AuctionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriber",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    AuctionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriber_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auctioneers_AuctionId",
                table: "Auctioneers",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "UserIdIndex",
                table: "Auctioneers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "OwnerUserIdIndex",
                table: "Auctions",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_AuctionId",
                table: "Lots",
                column: "AuctionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_AuctionId",
                table: "Subscriber",
                column: "AuctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auctioneers");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropTable(
                name: "Subscriber");

            migrationBuilder.DropTable(
                name: "Auctions");
        }
    }
}
