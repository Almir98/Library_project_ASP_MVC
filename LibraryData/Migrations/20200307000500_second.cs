using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryData.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibraryBranchID",
                table: "Patron",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LibraryCardID",
                table: "Patron",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LibraryBranches",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    OpenDate = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBranches", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LibraryCards",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fees = table.Column<decimal>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryCards", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BranchHours",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryBranchID = table.Column<int>(nullable: true),
                    DayOfWeek = table.Column<int>(nullable: false),
                    OpenTime = table.Column<int>(nullable: false),
                    CloseTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchHours", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BranchHours_LibraryBranches_LibraryBranchID",
                        column: x => x.LibraryBranchID,
                        principalTable: "LibraryBranches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LibraryAssets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    StatusID = table.Column<int>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    NumberOfCopies = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ISBN = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    DeweyIndex = table.Column<string>(nullable: true),
                    Director = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryAssets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LibraryAssets_LibraryBranches_LocationID",
                        column: x => x.LocationID,
                        principalTable: "LibraryBranches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LibraryAssets_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckOutHistories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryAssetID = table.Column<int>(nullable: false),
                    LibraryCardID = table.Column<int>(nullable: false),
                    CheckedOut = table.Column<DateTime>(nullable: false),
                    CheckedIn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOutHistories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CheckOutHistories_LibraryAssets_LibraryAssetID",
                        column: x => x.LibraryAssetID,
                        principalTable: "LibraryAssets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckOutHistories_LibraryCards_LibraryCardID",
                        column: x => x.LibraryCardID,
                        principalTable: "LibraryCards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckOuts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryAssetID = table.Column<int>(nullable: false),
                    LibraryCardID = table.Column<int>(nullable: true),
                    Since = table.Column<DateTime>(nullable: false),
                    Until = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOuts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CheckOuts_LibraryAssets_LibraryAssetID",
                        column: x => x.LibraryAssetID,
                        principalTable: "LibraryAssets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckOuts_LibraryCards_LibraryCardID",
                        column: x => x.LibraryCardID,
                        principalTable: "LibraryCards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Holds",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryAssetID = table.Column<int>(nullable: true),
                    LibraryCardID = table.Column<int>(nullable: true),
                    HoldPlaced = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holds", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Holds_LibraryAssets_LibraryAssetID",
                        column: x => x.LibraryAssetID,
                        principalTable: "LibraryAssets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holds_LibraryCards_LibraryCardID",
                        column: x => x.LibraryCardID,
                        principalTable: "LibraryCards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patron_LibraryBranchID",
                table: "Patron",
                column: "LibraryBranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Patron_LibraryCardID",
                table: "Patron",
                column: "LibraryCardID");

            migrationBuilder.CreateIndex(
                name: "IX_BranchHours_LibraryBranchID",
                table: "BranchHours",
                column: "LibraryBranchID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOutHistories_LibraryAssetID",
                table: "CheckOutHistories",
                column: "LibraryAssetID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOutHistories_LibraryCardID",
                table: "CheckOutHistories",
                column: "LibraryCardID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOuts_LibraryAssetID",
                table: "CheckOuts",
                column: "LibraryAssetID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOuts_LibraryCardID",
                table: "CheckOuts",
                column: "LibraryCardID");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_LibraryAssetID",
                table: "Holds",
                column: "LibraryAssetID");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_LibraryCardID",
                table: "Holds",
                column: "LibraryCardID");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAssets_LocationID",
                table: "LibraryAssets",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAssets_StatusID",
                table: "LibraryAssets",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Patron_LibraryBranches_LibraryBranchID",
                table: "Patron",
                column: "LibraryBranchID",
                principalTable: "LibraryBranches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patron_LibraryCards_LibraryCardID",
                table: "Patron",
                column: "LibraryCardID",
                principalTable: "LibraryCards",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patron_LibraryBranches_LibraryBranchID",
                table: "Patron");

            migrationBuilder.DropForeignKey(
                name: "FK_Patron_LibraryCards_LibraryCardID",
                table: "Patron");

            migrationBuilder.DropTable(
                name: "BranchHours");

            migrationBuilder.DropTable(
                name: "CheckOutHistories");

            migrationBuilder.DropTable(
                name: "CheckOuts");

            migrationBuilder.DropTable(
                name: "Holds");

            migrationBuilder.DropTable(
                name: "LibraryAssets");

            migrationBuilder.DropTable(
                name: "LibraryCards");

            migrationBuilder.DropTable(
                name: "LibraryBranches");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Patron_LibraryBranchID",
                table: "Patron");

            migrationBuilder.DropIndex(
                name: "IX_Patron_LibraryCardID",
                table: "Patron");

            migrationBuilder.DropColumn(
                name: "LibraryBranchID",
                table: "Patron");

            migrationBuilder.DropColumn(
                name: "LibraryCardID",
                table: "Patron");
        }
    }
}
