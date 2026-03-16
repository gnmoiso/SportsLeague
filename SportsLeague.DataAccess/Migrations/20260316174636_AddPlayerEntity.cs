using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsLeague.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure Teams table exists (handle cases where DB was dropped and initial migration not applied)
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[Teams]', N'U') IS NULL
BEGIN
    CREATE TABLE [Teams](
        [Id] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] nvarchar(100) NOT NULL,
        [City] nvarchar(100) NOT NULL,
        [Stadium] nvarchar(150) NOT NULL,
        [LogoUrl] nvarchar(500) NULL,
        [FoundedDate] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL
    );
    CREATE UNIQUE INDEX IX_Teams_Name ON [Teams]([Name]);
END");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId_Number",
                table: "Players",
                columns: new[] { "TeamId", "Number" },
                unique: true);

            // Add FK constraint only if Teams table exists to avoid migration failures when DB was reset
            migrationBuilder.Sql(@"IF OBJECT_ID(N'[dbo].[Teams]', N'U') IS NOT NULL
BEGIN
    IF OBJECT_ID(N'[dbo].[FK_Players_Teams_TeamId]', N'F') IS NULL
    BEGIN
        ALTER TABLE [Players] ADD CONSTRAINT [FK_Players_Teams_TeamId]
            FOREIGN KEY ([TeamId]) REFERENCES [Teams]([Id]) ON DELETE CASCADE;
    END
END");

            // Teams table already created by initial migration; only create Players here.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
