using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace NexumDevs.VitalTrek.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    expedition_id = table.Column<int>(type: "int", nullable: false),
                    tourist_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    severity = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    acknowledged_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    acknowledged_by = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_alerts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "binnacle_readings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    expedition_id = table.Column<int>(type: "int", nullable: false),
                    tourist_id = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "longtext", nullable: false),
                    media_url = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_binnacle_readings", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "expeditions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tour_i_d = table.Column<int>(type: "int", nullable: false),
                    guide_i_d = table.Column<int>(type: "int", nullable: false),
                    expedition_name = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_expeditions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GamificationProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TouristId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamificationProfiles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "location_readings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    expedition_id = table.Column<int>(type: "int", nullable: false),
                    tourist_id = table.Column<int>(type: "int", nullable: false),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    accuracy_meters = table.Column<double>(type: "double", nullable: false),
                    recorded_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_location_readings", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vital_sign_readings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    expedition_id = table.Column<int>(type: "int", nullable: false),
                    tourist_id = table.Column<int>(type: "int", nullable: false),
                    heart_rate = table.Column<int>(type: "int", nullable: false),
                    blood_oxygen = table.Column<double>(type: "double", nullable: false),
                    body_temperature = table.Column<double>(type: "double", nullable: false),
                    recorded_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vital_sign_readings", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "experiences",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    expedition_i_d = table.Column<int>(type: "int", nullable: false),
                    tourist_i_d = table.Column<int>(type: "int", nullable: false),
                    note = table.Column<string>(type: "longtext", nullable: false),
                    media_url = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_experiences", x => x.id);
                    table.ForeignKey(
                        name: "f_k_experiences_expeditions_expedition_i_d",
                        column: x => x.expedition_i_d,
                        principalTable: "expeditions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "progresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    expedition_id = table.Column<int>(type: "int", nullable: false),
                    completed_checkpoints = table.Column<int>(type: "int", nullable: false),
                    total_checkpoints = table.Column<int>(type: "int", nullable: false),
                    percentage = table.Column<double>(type: "double", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_progresses", x => x.id);
                    table.ForeignKey(
                        name: "f_k_progresses_expeditions_expedition_id",
                        column: x => x.expedition_id,
                        principalTable: "expeditions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "weathers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    expedition_id = table.Column<int>(type: "int", nullable: false),
                    temperature_celsius = table.Column<double>(type: "double", nullable: false),
                    condition = table.Column<string>(type: "longtext", nullable: false),
                    humidity = table.Column<double>(type: "double", nullable: false),
                    wind_speed_kmh = table.Column<double>(type: "double", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_weathers", x => x.id);
                    table.ForeignKey(
                        name: "f_k_weathers_expeditions_expedition_id",
                        column: x => x.expedition_id,
                        principalTable: "expeditions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AwardedExpeditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ExpeditionId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    AwardedAt = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwardedExpeditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AwardedExpeditions_GamificationProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "GamificationProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AwardedExpeditions_ProfileId_ExpeditionId",
                table: "AwardedExpeditions",
                columns: new[] { "ProfileId", "ExpeditionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_experiences_expedition_i_d",
                table: "experiences",
                column: "expedition_i_d");

            migrationBuilder.CreateIndex(
                name: "IX_GamificationProfiles_TouristId",
                table: "GamificationProfiles",
                column: "TouristId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_progresses_expedition_id",
                table: "progresses",
                column: "expedition_id");

            migrationBuilder.CreateIndex(
                name: "i_x_weathers_expedition_id",
                table: "weathers",
                column: "expedition_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "AwardedExpeditions");

            migrationBuilder.DropTable(
                name: "binnacle_readings");

            migrationBuilder.DropTable(
                name: "experiences");

            migrationBuilder.DropTable(
                name: "location_readings");

            migrationBuilder.DropTable(
                name: "progresses");

            migrationBuilder.DropTable(
                name: "vital_sign_readings");

            migrationBuilder.DropTable(
                name: "weathers");

            migrationBuilder.DropTable(
                name: "GamificationProfiles");

            migrationBuilder.DropTable(
                name: "expeditions");
        }
    }
}
