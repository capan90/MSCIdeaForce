using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSC.IdeaForge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdeaStatusAndFieldNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdeaStatus",
                table: "Problems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdeaStatusHistory",
                table: "Problems",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FieldNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    ObservationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    ProblemId = table.Column<Guid>(type: "uuid", nullable: true),
                    HasPhoto = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldNotes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldNotes");

            migrationBuilder.DropColumn(
                name: "IdeaStatus",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "IdeaStatusHistory",
                table: "Problems");
        }
    }
}
