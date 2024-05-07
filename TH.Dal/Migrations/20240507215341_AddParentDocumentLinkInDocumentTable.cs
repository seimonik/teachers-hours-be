using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TH.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddParentDocumentLinkInDocumentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "EndRow",
                table: "Documents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentDocumentId",
                table: "Documents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ParentDocumentId",
                table: "Documents",
                column: "ParentDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Documents_ParentDocumentId",
                table: "Documents",
                column: "ParentDocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Documents_ParentDocumentId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ParentDocumentId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "EndRow",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ParentDocumentId",
                table: "Documents");

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    EndRow = table.Column<int>(type: "integer", nullable: false),
                    ParentDocumentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });
        }
    }
}
