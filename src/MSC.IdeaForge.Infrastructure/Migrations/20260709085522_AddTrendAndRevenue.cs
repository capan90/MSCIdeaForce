using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSC.IdeaForge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrendAndRevenue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValidationQuestions",
                table: "Validations",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RevenueAnalyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProblemId = table.Column<Guid>(type: "uuid", nullable: false),
                    RevenueModel = table.Column<string>(type: "text", nullable: false),
                    MonthlyRevenueEstimate = table.Column<string>(type: "text", nullable: false),
                    AnnualRevenueEstimate = table.Column<string>(type: "text", nullable: false),
                    PricingStrategy = table.Column<string>(type: "text", nullable: false),
                    TargetCustomer = table.Column<string>(type: "text", nullable: false),
                    SalesChannel = table.Column<string>(type: "text", nullable: false),
                    Scalability = table.Column<string>(type: "text", nullable: false),
                    Risks = table.Column<string>(type: "text", nullable: false),
                    AnalyzedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueAnalyses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrendAnalyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrendName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    Opportunities = table.Column<string>(type: "text", nullable: false),
                    Actions = table.Column<string>(type: "text", nullable: false),
                    AnalyzedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrendAnalyses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevenueAnalyses");

            migrationBuilder.DropTable(
                name: "TrendAnalyses");

            migrationBuilder.DropColumn(
                name: "ValidationQuestions",
                table: "Validations");
        }
    }
}
