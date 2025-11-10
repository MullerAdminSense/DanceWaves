using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DanceWaves.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    GeoPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxContestants = table.Column<int>(type: "int", nullable: false),
                    RegistrationsOpenForMembers = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationsOpenForEveryone = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckInUntil = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfDancers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Franchises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VatNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsPartOfEU = table.Column<bool>(type: "bit", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SystemEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Styles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Styles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DanceSchools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VatNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsPartOfEU = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DefaultFranchiseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanceSchools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanceSchools_Franchises_DefaultFranchiseId",
                        column: x => x.DefaultFranchiseId,
                        principalTable: "Franchises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompetitionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    StyleId = table.Column<int>(type: "int", nullable: false),
                    AgeGroupId = table.Column<int>(type: "int", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    MinTeamSize = table.Column<int>(type: "int", nullable: false),
                    MaxTeamSize = table.Column<int>(type: "int", nullable: false),
                    GenderMix = table.Column<bool>(type: "bit", nullable: false),
                    MaxMusicLengthSeconds = table.Column<int>(type: "int", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionCategories_AgeGroups_AgeGroupId",
                        column: x => x.AgeGroupId,
                        principalTable: "AgeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionCategories_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionCategories_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionCategories_Styles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DanceSchoolId = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DefaultFranchiseId = table.Column<int>(type: "int", nullable: true),
                    AgeGroupId = table.Column<int>(type: "int", nullable: true),
                    RolePermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AgeGroups_AgeGroupId",
                        column: x => x.AgeGroupId,
                        principalTable: "AgeGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_DanceSchools_DanceSchoolId",
                        column: x => x.DanceSchoolId,
                        principalTable: "DanceSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Franchises_DefaultFranchiseId",
                        column: x => x.DefaultFranchiseId,
                        principalTable: "Franchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_UserRolePermissions_RolePermissionId",
                        column: x => x.RolePermissionId,
                        principalTable: "UserRolePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionCategoryId = table.Column<int>(type: "int", nullable: false),
                    StartNumber = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: true),
                    Song = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entries_CompetitionCategories_CompetitionCategoryId",
                        column: x => x.CompetitionCategoryId,
                        principalTable: "CompetitionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entries_DanceSchools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "DanceSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "JudgePanels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompetitionCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JudgePanels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JudgePanels_CompetitionCategories_CompetitionCategoryId",
                        column: x => x.CompetitionCategoryId,
                        principalTable: "CompetitionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JudgePanels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryMembers_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JudgeUserId = table.Column<int>(type: "int", nullable: false),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    RawScore = table.Column<int>(type: "int", nullable: false),
                    Penalty = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<int>(type: "int", nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scores_Users_JudgeUserId",
                        column: x => x.JudgeUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserRolePermissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Sees everything", "SuperAdmin" },
                    { 2, "Manages all connected users, contests, results", "FranchiseAdmin" },
                    { 3, "Sees his own data and joined contests", "User" },
                    { 4, "Can put results in the system per connected contest", "Jury" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCategories_AgeGroupId",
                table: "CompetitionCategories",
                column: "AgeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCategories_CompetitionId",
                table: "CompetitionCategories",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCategories_LevelId",
                table: "CompetitionCategories",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCategories_StyleId",
                table: "CompetitionCategories",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_DanceSchools_DefaultFranchiseId",
                table: "DanceSchools",
                column: "DefaultFranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CompetitionCategoryId",
                table: "Entries",
                column: "CompetitionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SchoolId",
                table: "Entries",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryMembers_EntryId",
                table: "EntryMembers",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryMembers_UserId",
                table: "EntryMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_VatNumber",
                table: "Franchises",
                column: "VatNumber");

            migrationBuilder.CreateIndex(
                name: "IX_JudgePanels_CompetitionCategoryId",
                table: "JudgePanels",
                column: "CompetitionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JudgePanels_UserId",
                table: "JudgePanels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_EntryId",
                table: "Scores",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_JudgeUserId",
                table: "Scores",
                column: "JudgeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermissions_Name",
                table: "UserRolePermissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AgeGroupId",
                table: "Users",
                column: "AgeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DanceSchoolId",
                table: "Users",
                column: "DanceSchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultFranchiseId",
                table: "Users",
                column: "DefaultFranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolePermissionId",
                table: "Users",
                column: "RolePermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryMembers");

            migrationBuilder.DropTable(
                name: "EntryTypes");

            migrationBuilder.DropTable(
                name: "JudgePanels");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CompetitionCategories");

            migrationBuilder.DropTable(
                name: "DanceSchools");

            migrationBuilder.DropTable(
                name: "UserRolePermissions");

            migrationBuilder.DropTable(
                name: "AgeGroups");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Styles");

            migrationBuilder.DropTable(
                name: "Franchises");
        }
    }
}
