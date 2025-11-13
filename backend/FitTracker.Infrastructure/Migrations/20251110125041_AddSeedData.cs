using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "exercise_records",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "exercise_records",
                newName: "created_at");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "exercise_records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "exercise_records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "exercises",
                columns: new[] { "id", "created_at", "description", "equipment", "image_url", "muscle_group", "name", "updated_at", "user_id", "video_url" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222221"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Compound chest exercise performed on a flat bench", 1, "https://example.com/exercises/bench-press.jpg", 0, "Barbell Bench Press", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://example.com/videos/bench-press.mp4" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fundamental lower body compound exercise", 1, "https://example.com/exercises/squat.jpg", 3, "Barbell Back Squat", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://example.com/videos/squat.mp4" },
                    { new Guid("22222222-2222-2222-2222-222222222223"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full body compound pulling exercise", 1, "https://example.com/exercises/deadlift.jpg", 1, "Conventional Deadlift", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://example.com/videos/deadlift.mp4" },
                    { new Guid("22222222-2222-2222-2222-222222222224"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Standing barbell shoulder press", 1, "https://example.com/exercises/ohp.jpg", 2, "Overhead Press", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://example.com/videos/ohp.mp4" },
                    { new Guid("22222222-2222-2222-2222-222222222225"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bodyweight vertical pulling exercise", 5, "https://example.com/exercises/pullups.jpg", 1, "Pull-Ups", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://example.com/videos/pullups.mp4" },
                    { new Guid("22222222-2222-2222-2222-222222222226"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bent-over barbell rowing exercise", 1, "https://example.com/exercises/row.jpg", 1, "Barbell Row", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://example.com/videos/row.mp4" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "avatar", "bio", "created_at", "email", "first_name", "last_name", "password_hash", "preferred_units", "updated_at", "username" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "https://example.com/avatars/john.jpg", "Passionate about fitness and strength training. 5 years experience.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fitnesspro@example.com", "John", "Athlete", "$2a$11$X5wFuQE5cCcYKfZ1EE.IbeQQfFhVxR4rL8CxKgE8X9Y.wU3jZ9r4C", "metric", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fitness_pro" });

            migrationBuilder.InsertData(
                table: "achievements",
                columns: new[] { "id", "created_at", "description", "icon_url", "is_unlocked", "name", "progress", "target", "tier", "type", "unlocked_at", "updated_at", "user_id" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Complete 7 consecutive days of workouts", "https://example.com/icons/streak.png", true, "Consistency King", 7, 7, 2, 1, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Complete 100 total workouts", "https://example.com/icons/century.png", false, "Century Club", 63, 100, 3, 2, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Lift a total of 50,000 kg", "https://example.com/icons/iron.png", true, "Iron Warrior", 50000, 50000, 3, 3, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Set 20 personal records", "https://example.com/icons/pr.png", false, "Record Breaker", 12, 20, 2, 4, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Complete your first workout", "https://example.com/icons/first-workout.png", true, "First Steps", 1, 1, 1, 0, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "exercise_records",
                columns: new[] { "id", "created_at", "exercise_id", "last_performed", "max_reps", "max_reps_date", "max_total_volume", "max_total_volume_date", "max_volume", "max_volume_date", "max_weight_date", "max_weight_kg", "total_lifted", "total_reps", "total_sets", "total_workouts", "updated_at", "user_id" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999991"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222221"), new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), 12, new DateTime(2024, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), 4500.0m, new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), 960.0m, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 100.0m, 42000.0m, 576, 72, 24, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("99999999-9999-9999-9999-999999999992"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 10, new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5200.0m, new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 1200.0m, new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), 140.0m, 58000.0m, 504, 63, 21, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("99999999-9999-9999-9999-999999999993"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222223"), new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 8, new DateTime(2024, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 4800.0m, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1280.0m, new DateTime(2024, 10, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 180.0m, 48000.0m, 324, 54, 18, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("99999999-9999-9999-9999-999999999994"), new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222224"), new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), 10, new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2100.0m, new DateTime(2024, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), 520.0m, new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 65.0m, 28000.0m, 480, 60, 20, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "workout_templates",
                columns: new[] { "id", "created_at", "description", "last_used_at", "name", "updated_at", "usage_count", "user_id" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333331"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chest, shoulders, and triceps workout", new DateTime(2024, 11, 8, 10, 0, 0, 0, DateTimeKind.Utc), "Push Day", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333332"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Back and biceps workout", new DateTime(2024, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), "Pull Day", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lower body workout", new DateTime(2024, 11, 7, 10, 0, 0, 0, DateTimeKind.Utc), "Leg Day", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "workout_template_exercises",
                columns: new[] { "id", "created_at", "exercise_id", "notes", "order_index", "updated_at", "workout_template_id" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555551"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222221"), "Focus on controlled descent", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333331") },
                    { new Guid("55555555-5555-5555-5555-555555555552"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222224"), null, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333331") },
                    { new Guid("55555555-5555-5555-5555-555555555553"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222225"), "Add weight if possible", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333332") },
                    { new Guid("55555555-5555-5555-5555-555555555554"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222226"), null, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333332") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), "Go deep", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("55555555-5555-5555-5555-555555555556"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222223"), "Keep back neutral", 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333") }
                });

            migrationBuilder.InsertData(
                table: "workouts",
                columns: new[] { "id", "completed_at", "created_at", "duration", "is_completed", "is_in_progress", "name", "notes", "started_at", "total_volume_kg", "updated_at", "user_id", "workout_date", "workout_template_id" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444441"), new DateTime(2024, 11, 8, 11, 15, 30, 0, DateTimeKind.Utc), new DateTime(2024, 11, 8, 10, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 1, 15, 30, 0), true, false, "Push Day - Nov 8", "Great workout, felt strong", new DateTime(2024, 11, 8, 10, 0, 0, 0, DateTimeKind.Utc), 1450.50m, new DateTime(2024, 11, 8, 11, 15, 30, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 8, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333331") },
                    { new Guid("44444444-4444-4444-4444-444444444442"), new DateTime(2024, 11, 9, 11, 5, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 1, 5, 0, 0), true, false, "Pull Day - Nov 9", null, new DateTime(2024, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), 1280.00m, new DateTime(2024, 11, 9, 11, 5, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333332") }
                });

            migrationBuilder.InsertData(
                table: "template_sets",
                columns: new[] { "id", "created_at", "planned_reps", "planned_weight", "rest_seconds", "set_number", "set_type", "updated_at", "workout_template_exercise_id" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666601"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, 60.0m, 120, 1, 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("55555555-5555-5555-5555-555555555551") },
                    { new Guid("66666666-6666-6666-6666-666666666602"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, 70.0m, 120, 2, 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("55555555-5555-5555-5555-555555555551") },
                    { new Guid("66666666-6666-6666-6666-666666666603"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 80.0m, 180, 3, 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("55555555-5555-5555-5555-555555555551") },
                    { new Guid("66666666-6666-6666-6666-666666666604"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, 40.0m, 90, 1, 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("55555555-5555-5555-5555-555555555552") },
                    { new Guid("66666666-6666-6666-6666-666666666605"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, 45.0m, 90, 2, 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("55555555-5555-5555-5555-555555555552") },
                    { new Guid("66666666-6666-6666-6666-666666666606"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 50.0m, 120, 3, 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("55555555-5555-5555-5555-555555555552") }
                });

            migrationBuilder.InsertData(
                table: "workout_exercises",
                columns: new[] { "id", "created_at", "exercise_id", "notes", "order_index", "updated_at", "workout_id" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777771"), new DateTime(2024, 11, 8, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222221"), "Good form today", 1, new DateTime(2024, 11, 8, 10, 30, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444441") },
                    { new Guid("77777777-7777-7777-7777-777777777772"), new DateTime(2024, 11, 8, 10, 35, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222224"), null, 2, new DateTime(2024, 11, 8, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444441") },
                    { new Guid("77777777-7777-7777-7777-777777777773"), new DateTime(2024, 11, 9, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222225"), "Added 10kg weight", 1, new DateTime(2024, 11, 9, 10, 25, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444442") },
                    { new Guid("77777777-7777-7777-7777-777777777774"), new DateTime(2024, 11, 9, 10, 30, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222226"), null, 2, new DateTime(2024, 11, 9, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444442") }
                });

            migrationBuilder.InsertData(
                table: "sets",
                columns: new[] { "id", "completed_at", "created_at", "is_completed", "reps", "rest_seconds", "set_number", "set_type", "updated_at", "weight_kg", "workout_exercise_id" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888801"), new DateTime(2024, 11, 8, 10, 5, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 8, 10, 3, 0, 0, DateTimeKind.Utc), true, 10, 120, 1, 0, new DateTime(2024, 11, 8, 10, 5, 0, 0, DateTimeKind.Utc), 60.0m, new Guid("77777777-7777-7777-7777-777777777771") },
                    { new Guid("88888888-8888-8888-8888-888888888802"), new DateTime(2024, 11, 8, 10, 10, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 8, 10, 7, 0, 0, DateTimeKind.Utc), true, 8, 120, 2, 0, new DateTime(2024, 11, 8, 10, 10, 0, 0, DateTimeKind.Utc), 70.0m, new Guid("77777777-7777-7777-7777-777777777771") },
                    { new Guid("88888888-8888-8888-8888-888888888803"), new DateTime(2024, 11, 8, 10, 15, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 8, 10, 12, 0, 0, DateTimeKind.Utc), true, 6, 180, 3, 0, new DateTime(2024, 11, 8, 10, 15, 0, 0, DateTimeKind.Utc), 80.0m, new Guid("77777777-7777-7777-7777-777777777771") },
                    { new Guid("88888888-8888-8888-8888-888888888804"), new DateTime(2024, 11, 8, 10, 40, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 8, 10, 38, 0, 0, DateTimeKind.Utc), true, 10, 90, 1, 0, new DateTime(2024, 11, 8, 10, 40, 0, 0, DateTimeKind.Utc), 40.0m, new Guid("77777777-7777-7777-7777-777777777772") },
                    { new Guid("88888888-8888-8888-8888-888888888805"), new DateTime(2024, 11, 8, 10, 45, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 8, 10, 42, 0, 0, DateTimeKind.Utc), true, 8, 90, 2, 0, new DateTime(2024, 11, 8, 10, 45, 0, 0, DateTimeKind.Utc), 45.0m, new Guid("77777777-7777-7777-7777-777777777772") },
                    { new Guid("88888888-8888-8888-8888-888888888806"), new DateTime(2024, 11, 9, 10, 5, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 9, 10, 3, 0, 0, DateTimeKind.Utc), true, 8, 120, 1, 0, new DateTime(2024, 11, 9, 10, 5, 0, 0, DateTimeKind.Utc), 10.0m, new Guid("77777777-7777-7777-7777-777777777773") },
                    { new Guid("88888888-8888-8888-8888-888888888807"), new DateTime(2024, 11, 9, 10, 10, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 9, 10, 7, 0, 0, DateTimeKind.Utc), true, 7, 120, 2, 0, new DateTime(2024, 11, 9, 10, 10, 0, 0, DateTimeKind.Utc), 10.0m, new Guid("77777777-7777-7777-7777-777777777773") },
                    { new Guid("88888888-8888-8888-8888-888888888808"), new DateTime(2024, 11, 9, 10, 35, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 9, 10, 33, 0, 0, DateTimeKind.Utc), true, 8, 90, 1, 0, new DateTime(2024, 11, 9, 10, 35, 0, 0, DateTimeKind.Utc), 80.0m, new Guid("77777777-7777-7777-7777-777777777774") },
                    { new Guid("88888888-8888-8888-8888-888888888809"), new DateTime(2024, 11, 9, 10, 40, 0, 0, DateTimeKind.Utc), new DateTime(2024, 11, 9, 10, 37, 0, 0, DateTimeKind.Utc), true, 6, 90, 2, 0, new DateTime(2024, 11, 9, 10, 40, 0, 0, DateTimeKind.Utc), 85.0m, new Guid("77777777-7777-7777-7777-777777777774") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"));

            migrationBuilder.DeleteData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"));

            migrationBuilder.DeleteData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"));

            migrationBuilder.DeleteData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"));

            migrationBuilder.DeleteData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999991"));

            migrationBuilder.DeleteData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999992"));

            migrationBuilder.DeleteData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999993"));

            migrationBuilder.DeleteData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999994"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888801"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888802"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888803"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888804"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888805"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888806"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888807"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888808"));

            migrationBuilder.DeleteData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888809"));

            migrationBuilder.DeleteData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666601"));

            migrationBuilder.DeleteData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666602"));

            migrationBuilder.DeleteData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666603"));

            migrationBuilder.DeleteData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666604"));

            migrationBuilder.DeleteData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666605"));

            migrationBuilder.DeleteData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666606"));

            migrationBuilder.DeleteData(
                table: "workout_template_exercises",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555553"));

            migrationBuilder.DeleteData(
                table: "workout_template_exercises",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555554"));

            migrationBuilder.DeleteData(
                table: "workout_template_exercises",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "workout_template_exercises",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555556"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222223"));

            migrationBuilder.DeleteData(
                table: "workout_exercises",
                keyColumn: "id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777771"));

            migrationBuilder.DeleteData(
                table: "workout_exercises",
                keyColumn: "id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777772"));

            migrationBuilder.DeleteData(
                table: "workout_exercises",
                keyColumn: "id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777773"));

            migrationBuilder.DeleteData(
                table: "workout_exercises",
                keyColumn: "id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777774"));

            migrationBuilder.DeleteData(
                table: "workout_template_exercises",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555551"));

            migrationBuilder.DeleteData(
                table: "workout_template_exercises",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555552"));

            migrationBuilder.DeleteData(
                table: "workout_templates",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222224"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222225"));

            migrationBuilder.DeleteData(
                table: "exercises",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222226"));

            migrationBuilder.DeleteData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"));

            migrationBuilder.DeleteData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"));

            migrationBuilder.DeleteData(
                table: "workout_templates",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333331"));

            migrationBuilder.DeleteData(
                table: "workout_templates",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333332"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "exercise_records",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "exercise_records",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "exercise_records",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "exercise_records",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
        }
    }
}
