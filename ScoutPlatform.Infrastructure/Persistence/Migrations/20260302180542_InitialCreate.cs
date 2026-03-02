using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScoutPlatform.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetricDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Unit = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    HigherIsBetter = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Group = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    NormalizationStrategy = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    MinExpected = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    MaxExpected = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    PrimaryPosition = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CurrentClub = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MarketValueEur = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuitabilityScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    ScoreVersion = table.Column<int>(type: "integer", nullable: false),
                    BreakdownJson = table.Column<string>(type: "jsonb", nullable: false),
                    CalculatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuitabilityScores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Style = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    TargetPosition = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BudgetMaxEur = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MinMinutesPlayed = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeasonId = table.Column<int>(type: "integer", nullable: false),
                    CompetitionId = table.Column<string>(type: "text", nullable: true),
                    MetricKey = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    Minutes = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Source = table.Column<string>(type: "text", nullable: true),
                    CollectedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMetrics_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamProfileWeights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeamProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetricKey = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    IsHardConstraint = table.Column<bool>(type: "boolean", nullable: false),
                    MinValue = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    MaxValue = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamProfileWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamProfileWeights_TeamProfiles_TeamProfileId",
                        column: x => x.TeamProfileId,
                        principalTable: "TeamProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MetricDefinitions",
                columns: new[] { "Id", "CreatedAtUtc", "Description", "Group", "HigherIsBetter", "Key", "MaxExpected", "MinExpected", "Name", "NormalizationStrategy", "Unit", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("9f37f1d1-6433-4f4b-b57d-47b503c9d101"), new DateTime(2026, 3, 2, 18, 5, 42, 191, DateTimeKind.Utc).AddTicks(2836), "Expected goals per 90 minutes", "Attacking", true, "xg_per90", 0.60m, 0.05m, "xG Per 90", "MinMax", "xG", null },
                    { new Guid("9f37f1d1-6433-4f4b-b57d-47b503c9d102"), new DateTime(2026, 3, 2, 18, 5, 42, 191, DateTimeKind.Utc).AddTicks(2860), "Pressuring actions per 90 minutes", "Defending", true, "pressures_per90", 25m, 5m, "Pressures Per 90", "MinMax", "count", null },
                    { new Guid("9f37f1d1-6433-4f4b-b57d-47b503c9d103"), new DateTime(2026, 3, 2, 18, 5, 42, 191, DateTimeKind.Utc).AddTicks(2867), "Minutes played in season", "Availability", true, "minutes", 3200m, 0m, "Minutes Played", "MinMax", "minutes", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetricDefinitions_Key",
                table: "MetricDefinitions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMetrics_PlayerId_SeasonId_MetricKey",
                table: "PlayerMetrics",
                columns: new[] { "PlayerId", "SeasonId", "MetricKey" });

            migrationBuilder.CreateIndex(
                name: "IX_SuitabilityScores_TeamProfileId_PlayerId_ScoreVersion",
                table: "SuitabilityScores",
                columns: new[] { "TeamProfileId", "PlayerId", "ScoreVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamProfiles_OrganizationId_Name",
                table: "TeamProfiles",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamProfileWeights_TeamProfileId_MetricKey",
                table: "TeamProfileWeights",
                columns: new[] { "TeamProfileId", "MetricKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetricDefinitions");

            migrationBuilder.DropTable(
                name: "PlayerMetrics");

            migrationBuilder.DropTable(
                name: "SuitabilityScores");

            migrationBuilder.DropTable(
                name: "TeamProfileWeights");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "TeamProfiles");
        }
    }
}
