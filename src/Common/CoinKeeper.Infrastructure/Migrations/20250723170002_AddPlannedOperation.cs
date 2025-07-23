using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoinKeeper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlannedOperation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlannedOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OperationType = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NextExecutionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Frequency = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    FrequencyType = table.Column<int>(type: "integer", nullable: false),
                    ScheduledTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MaxExecutions = table.Column<int>(type: "integer", nullable: true),
                    ExecutedCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsPaused = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedOperations_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlannedOperations_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlannedOperations_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlannedOperations_AccountId",
                table: "PlannedOperations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedOperations_CategoryId",
                table: "PlannedOperations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedOperations_EndDate",
                table: "PlannedOperations",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedOperations_IsActive_IsDeleted_IsPaused",
                table: "PlannedOperations",
                columns: ["IsActive", "IsDeleted", "IsPaused"]);

            migrationBuilder.CreateIndex(
                name: "IX_PlannedOperations_NextExecutionDate",
                table: "PlannedOperations",
                column: "NextExecutionDate");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedOperations_StartDate",
                table: "PlannedOperations",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedOperations_UserId",
                table: "PlannedOperations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannedOperations");
        }
    }
}
