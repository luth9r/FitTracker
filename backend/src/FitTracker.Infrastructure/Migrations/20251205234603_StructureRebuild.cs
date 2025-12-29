using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTracker.Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class StructureRebuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_achievements_users_user_id",
                table: "achievements");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_exercises_users_user_id",
                table: "exercises");

            _ = migrationBuilder.DropIndex(
                name: "IX_Exercises_Equipment",
                table: "exercises");

            _ = migrationBuilder.DropIndex(
                name: "IX_Exercises_User_Custom",
                table: "exercises");

            _ = migrationBuilder.DropIndex(
                name: "IX_Achievements_IsUnlocked",
                table: "achievements");

            _ = migrationBuilder.DropIndex(
                name: "IX_Achievements_User_Type",
                table: "achievements");

            _ = migrationBuilder.DropIndex(
                name: "IX_Achievements_UserId",
                table: "achievements");

            _ = migrationBuilder.DropColumn(
                name: "is_custom",
                table: "exercises");

            _ = migrationBuilder.DropColumn(
                name: "is_unlocked",
                table: "achievements");

            _ = migrationBuilder.DropColumn(
                name: "progress",
                table: "achievements");

            _ = migrationBuilder.DropColumn(
                name: "unlocked_at",
                table: "achievements");

            _ = migrationBuilder.DropColumn(
                name: "user_id",
                table: "achievements");

            _ = migrationBuilder.RenameColumn(
                name: "id",
                table: "exercises",
                newName: "Id");

            _ = migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "exercises",
                newName: "UpdatedAt");

            _ = migrationBuilder.RenameColumn(
                name: "created_at",
                table: "exercises",
                newName: "CreatedAt");

            _ = migrationBuilder.RenameColumn(
                name: "user_id",
                table: "exercises",
                newName: "created_by_user_id");

            _ = migrationBuilder.RenameIndex(
                name: "IX_Exercises_Name",
                table: "exercises",
                newName: "IX_exercises_name");

            _ = migrationBuilder.RenameIndex(
                name: "IX_Exercises_MuscleGroup",
                table: "exercises",
                newName: "IX_exercises_muscle_group");

            _ = migrationBuilder.AlterColumn<string>(
                name: "preferred_units",
                table: "users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "metric",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "exercises",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "exercises",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            _ = migrationBuilder.CreateTable(
                name: "user_achievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    achievement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    progress = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_unlocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    unlocked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_user_achievements", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_user_achievements_achievements_achievement_id",
                        column: x => x.achievement_id,
                        principalTable: "achievements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "FK_user_achievements_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "created_at", "icon_url", "tier", "updated_at" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/icons/achievement_streak.png", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "created_at", "icon_url", "tier", "updated_at" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/icons/achievement_century.png", 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "created_at", "icon_url", "tier", "updated_at" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/icons/achievement_iron.png", 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                columns: new[] { "created_at", "icon_url", "tier", "updated_at" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/icons/achievement_pr.png", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "created_at", "icon_url", "tier", "updated_at" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/icons/achievement_first.png", 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { null, null });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { null, null });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { null, null });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222224"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { null, null });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222225"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { null, null });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222226"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { null, null });

            _ = migrationBuilder.InsertData(
                table: "exercises",
                columns: new[] { "Id", "CreatedAt", "created_by_user_id", "description", "equipment", "image_url", "muscle_group", "name", "UpdatedAt", "video_url" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222227"), new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), "Custom bicep curl variation", 2, null, 4, "John's Special Curl", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), null });

            _ = migrationBuilder.InsertData(
                table: "user_achievements",
                columns: new[] { "Id", "achievement_id", "CreatedAt", "is_unlocked", "progress", "unlocked_at", "UpdatedAt", "user_id" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, 7, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.InsertData(
                table: "user_achievements",
                columns: new[] { "Id", "achievement_id", "CreatedAt", "progress", "unlocked_at", "UpdatedAt", "user_id" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 63, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.InsertData(
                table: "user_achievements",
                columns: new[] { "Id", "achievement_id", "CreatedAt", "is_unlocked", "progress", "unlocked_at", "UpdatedAt", "user_id" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, 50000, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.InsertData(
                table: "user_achievements",
                columns: new[] { "Id", "achievement_id", "CreatedAt", "progress", "unlocked_at", "UpdatedAt", "user_id" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb4"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 12, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.InsertData(
                table: "user_achievements",
                columns: new[] { "Id", "achievement_id", "CreatedAt", "is_unlocked", "progress", "unlocked_at", "UpdatedAt", "user_id" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "is_email_verified",
                value: true);

            _ = migrationBuilder.UpdateData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                column: "workout_date",
                value: new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            _ = migrationBuilder.UpdateData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                column: "workout_date",
                value: new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc));

            _ = migrationBuilder.CreateIndex(
                name: "IX_exercises_standard",
                table: "exercises",
                column: "created_by_user_id",
                filter: "created_by_user_id IS NULL");

            _ = migrationBuilder.CreateIndex(
                name: "IX_user_achievements_achievement_id",
                table: "user_achievements",
                column: "achievement_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_user_achievements_user_id",
                table: "user_achievements",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_user_achievements_user_id_achievement_id",
                table: "user_achievements",
                columns: new[] { "user_id", "achievement_id" },
                unique: true);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_exercises_users_created_by_user_id",
                table: "exercises",
                column: "created_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_exercises_users_created_by_user_id",
                table: "exercises");

            _ = migrationBuilder.DropTable(
                name: "user_achievements");

            _ = migrationBuilder.DropIndex(
                name: "IX_exercises_standard",
                table: "exercises");

            _ = migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222227"));

            _ = migrationBuilder.RenameColumn(
                name: "Id",
                table: "exercises",
                newName: "id");

            _ = migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "exercises",
                newName: "updated_at");

            _ = migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "exercises",
                newName: "created_at");

            _ = migrationBuilder.RenameColumn(
                name: "created_by_user_id",
                table: "exercises",
                newName: "user_id");

            _ = migrationBuilder.RenameIndex(
                name: "IX_exercises_name",
                table: "exercises",
                newName: "IX_Exercises_Name");

            _ = migrationBuilder.RenameIndex(
                name: "IX_exercises_muscle_group",
                table: "exercises",
                newName: "IX_Exercises_MuscleGroup");

            _ = migrationBuilder.AlterColumn<string>(
                name: "preferred_units",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldDefaultValue: "metric");

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "exercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "exercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            _ = migrationBuilder.AddColumn<bool>(
                name: "is_custom",
                table: "exercises",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "is_unlocked",
                table: "achievements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<int>(
                name: "progress",
                table: "achievements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "unlocked_at",
                table: "achievements",
                type: "timestamp with time zone",
                nullable: true);

            _ = migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "achievements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                columns: new[] { "created_at", "icon_url", "is_unlocked", "progress", "tier", "unlocked_at", "updated_at", "user_id" },
                values: new object[] { new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "https://example.com/icons/streak.png", true, 7, 2, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                columns: new[] { "created_at", "icon_url", "is_unlocked", "progress", "tier", "unlocked_at", "updated_at", "user_id" },
                values: new object[] { new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "https://example.com/icons/century.png", false, 63, 3, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                columns: new[] { "created_at", "icon_url", "is_unlocked", "progress", "tier", "unlocked_at", "updated_at", "user_id" },
                values: new object[] { new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "https://example.com/icons/iron.png", true, 50000, 3, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                columns: new[] { "created_at", "icon_url", "is_unlocked", "progress", "tier", "unlocked_at", "updated_at", "user_id" },
                values: new object[] { new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "https://example.com/icons/pr.png", false, 12, 2, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "created_at", "icon_url", "is_unlocked", "progress", "tier", "unlocked_at", "updated_at", "user_id" },
                values: new object[] { new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "https://example.com/icons/first-workout.png", true, 1, 1, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { "https://example.com/exercises/bench-press.jpg", "https://example.com/videos/bench-press.mp4" });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { "https://example.com/exercises/squat.jpg", "https://example.com/videos/squat.mp4" });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { "https://example.com/exercises/deadlift.jpg", "https://example.com/videos/deadlift.mp4" });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222224"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { "https://example.com/exercises/ohp.jpg", "https://example.com/videos/ohp.mp4" });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222225"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { "https://example.com/exercises/pullups.jpg", "https://example.com/videos/pullups.mp4" });

            _ = migrationBuilder.UpdateData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222226"),
                columns: new[] { "image_url", "video_url" },
                values: new object[] { "https://example.com/exercises/row.jpg", "https://example.com/videos/row.mp4" });

            _ = migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "is_email_verified",
                value: false);

            _ = migrationBuilder.UpdateData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                column: "workout_date",
                value: new DateTime(2024, 11, 8, 10, 0, 0, 0, DateTimeKind.Utc));

            _ = migrationBuilder.UpdateData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                column: "workout_date",
                value: new DateTime(2024, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc));

            _ = migrationBuilder.CreateIndex(
                name: "IX_Exercises_Equipment",
                table: "exercises",
                column: "equipment");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Exercises_User_Custom",
                table: "exercises",
                columns: new[] { "user_id", "is_custom" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Achievements_IsUnlocked",
                table: "achievements",
                column: "is_unlocked");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Achievements_User_Type",
                table: "achievements",
                columns: new[] { "user_id", "type" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Achievements_UserId",
                table: "achievements",
                column: "user_id");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_achievements_users_user_id",
                table: "achievements",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_exercises_users_user_id",
                table: "exercises",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
