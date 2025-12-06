using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    avatar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    bio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    preferred_units = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_users", x => x.id);
                });

            _ = migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    icon_url = table.Column<string>(type: "text", nullable: false),
                    progress = table.Column<int>(type: "integer", nullable: false),
                    target = table.Column<int>(type: "integer", nullable: false),
                    is_unlocked = table.Column<bool>(type: "boolean", nullable: false),
                    unlocked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tier = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_achievements", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_achievements_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "exercises",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    video_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    muscle_group = table.Column<int>(type: "integer", nullable: false),
                    equipment = table.Column<int>(type: "integer", nullable: false),
                    is_custom = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_exercises", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_exercises_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            _ = migrationBuilder.CreateTable(
                name: "workout_templates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    usage_count = table.Column<int>(type: "integer", nullable: false),
                    last_used_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_workout_templates", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_workout_templates_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "exercise_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    max_weight_kg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    max_reps = table.Column<int>(type: "integer", nullable: false),
                    max_volume = table.Column<decimal>(type: "numeric", nullable: false),
                    max_total_volume = table.Column<decimal>(type: "numeric", nullable: false),
                    max_weight_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    max_reps_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    max_volume_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    max_total_volume_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_workouts = table.Column<int>(type: "integer", nullable: false),
                    total_sets = table.Column<int>(type: "integer", nullable: false),
                    total_reps = table.Column<int>(type: "integer", nullable: false),
                    total_lifted = table.Column<decimal>(type: "numeric", nullable: false),
                    last_performed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_exercise_records", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_exercise_records_exercises_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_exercise_records_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "workout_template_exercises",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_workout_template_exercises", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_workout_template_exercises_exercises_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "FK_workout_template_exercises_workout_templates_workout_templa~",
                        column: x => x.workout_template_id,
                        principalTable: "workout_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "workouts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_template_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    workout_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    is_in_progress = table.Column<bool>(type: "boolean", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    total_volume_kg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_workouts", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_workouts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_workouts_workout_templates_workout_template_id",
                        column: x => x.workout_template_id,
                        principalTable: "workout_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            _ = migrationBuilder.CreateTable(
                name: "template_sets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_template_exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    set_number = table.Column<int>(type: "integer", nullable: false),
                    planned_weight = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    planned_reps = table.Column<int>(type: "integer", nullable: false),
                    rest_seconds = table.Column<int>(type: "integer", nullable: true),
                    set_type = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_template_sets", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_template_sets_workout_template_exercises_workout_template_e~",
                        column: x => x.workout_template_exercise_id,
                        principalTable: "workout_template_exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "workout_exercises",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_workout_exercises", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_workout_exercises_exercises_exercise_id",
                        column: x => x.exercise_id,
                        principalTable: "exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "FK_workout_exercises_workouts_workout_id",
                        column: x => x.workout_id,
                        principalTable: "workouts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "sets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    set_number = table.Column<int>(type: "integer", nullable: false),
                    weight_kg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    reps = table.Column<int>(type: "integer", nullable: false),
                    rest_seconds = table.Column<int>(type: "integer", nullable: true),
                    set_type = table.Column<int>(type: "integer", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_sets", x => x.id);
                    _ = table.ForeignKey(
                        name: "FK_sets_workout_exercises_workout_exercise_id",
                        column: x => x.workout_exercise_id,
                        principalTable: "workout_exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            _ = migrationBuilder.CreateIndex(
                name: "IX_exercise_records_exercise_id",
                table: "exercise_records",
                column: "exercise_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ExerciseRecords_LastPerformed",
                table: "exercise_records",
                column: "last_performed");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ExerciseRecords_User_Exercise",
                table: "exercise_records",
                columns: new[] { "user_id", "exercise_id" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Exercises_Equipment",
                table: "exercises",
                column: "equipment");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Exercises_MuscleGroup",
                table: "exercises",
                column: "muscle_group");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name",
                table: "exercises",
                column: "name");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Exercises_User_Custom",
                table: "exercises",
                columns: new[] { "user_id", "is_custom" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Sets_IsCompleted",
                table: "sets",
                column: "is_completed");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Sets_WorkoutExercise_SetNumber",
                table: "sets",
                columns: new[] { "workout_exercise_id", "set_number" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Sets_WorkoutExerciseId",
                table: "sets",
                column: "workout_exercise_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TemplateSets_TemplateExercise_SetNumber",
                table: "template_sets",
                columns: new[] { "workout_template_exercise_id", "set_number" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_TemplateSets_TemplateExerciseId",
                table: "template_sets",
                column: "workout_template_exercise_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "users",
                column: "email",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "users",
                column: "username",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "workout_exercises",
                column: "exercise_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_Workout_Order",
                table: "workout_exercises",
                columns: new[] { "workout_id", "order_index" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_WorkoutId",
                table: "workout_exercises",
                column: "workout_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercises_ExerciseId",
                table: "workout_template_exercises",
                column: "exercise_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercises_Template_Order",
                table: "workout_template_exercises",
                columns: new[] { "workout_template_id", "order_index" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplateExercises_TemplateId",
                table: "workout_template_exercises",
                column: "workout_template_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_User_LastUsed",
                table: "workout_templates",
                columns: new[] { "user_id", "last_used_at" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutTemplates_UserId",
                table: "workout_templates",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Workouts_IsCompleted",
                table: "workouts",
                column: "is_completed");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Workouts_TemplateId",
                table: "workouts",
                column: "workout_template_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Workouts_User_Date",
                table: "workouts",
                columns: new[] { "user_id", "workout_date" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Workouts_UserId",
                table: "workouts",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Workouts_WorkoutDate",
                table: "workouts",
                column: "workout_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "achievements");

            _ = migrationBuilder.DropTable(
                name: "exercise_records");

            _ = migrationBuilder.DropTable(
                name: "sets");

            _ = migrationBuilder.DropTable(
                name: "template_sets");

            _ = migrationBuilder.DropTable(
                name: "workout_exercises");

            _ = migrationBuilder.DropTable(
                name: "workout_template_exercises");

            _ = migrationBuilder.DropTable(
                name: "workouts");

            _ = migrationBuilder.DropTable(
                name: "exercises");

            _ = migrationBuilder.DropTable(
                name: "workout_templates");

            _ = migrationBuilder.DropTable(
                name: "users");
        }
    }
}
