using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTracker.Infrastructure.Migrations
{
    [ExcludeFromCodeCoverage]

    /// <inheritdoc />
    public partial class DeletePrefUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preferred_units",
                table: "users");

            migrationBuilder.AlterColumn<double>(
                name: "total_volume_kg",
                table: "workouts",
                type: "double precision",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "weight_kg",
                table: "sets",
                type: "double precision",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "total_lifted",
                table: "exercise_records",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "max_weight_kg",
                table: "exercise_records",
                type: "double precision",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "max_volume",
                table: "exercise_records",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "max_total_volume",
                table: "exercise_records",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.UpdateData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999991"),
                columns: new[] { "max_total_volume", "max_volume", "max_weight_kg", "total_lifted" },
                values: new object[] { 4500.0, 960.0, 100.0, 42000.0 });

            migrationBuilder.UpdateData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999992"),
                columns: new[] { "max_total_volume", "max_volume", "max_weight_kg", "total_lifted" },
                values: new object[] { 5200.0, 1200.0, 140.0, 58000.0 });

            migrationBuilder.UpdateData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999993"),
                columns: new[] { "max_total_volume", "max_volume", "max_weight_kg", "total_lifted" },
                values: new object[] { 4800.0, 1280.0, 180.0, 48000.0 });

            migrationBuilder.UpdateData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999994"),
                columns: new[] { "max_total_volume", "max_volume", "max_weight_kg", "total_lifted" },
                values: new object[] { 2100.0, 520.0, 65.0, 28000.0 });

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888801"),
                column: "weight_kg",
                value: 60.0);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888802"),
                column: "weight_kg",
                value: 70.0);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888803"),
                column: "weight_kg",
                value: 80.0);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888804"),
                column: "weight_kg",
                value: 40.0);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888805"),
                column: "weight_kg",
                value: 45.0);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888806"),
                column: "weight_kg",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888807"),
                column: "weight_kg",
                value: 10.0);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888808"),
                column: "weight_kg",
                value: 80.0);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888809"),
                column: "weight_kg",
                value: 85.0);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666601"),
                column: "planned_weight",
                value: 60.0);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666602"),
                column: "planned_weight",
                value: 70.0);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666603"),
                column: "planned_weight",
                value: 80.0);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666604"),
                column: "planned_weight",
                value: 40.0);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666605"),
                column: "planned_weight",
                value: 45.0);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666606"),
                column: "planned_weight",
                value: 50.0);

            migrationBuilder.UpdateData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                column: "total_volume_kg",
                value: 1450.5);

            migrationBuilder.UpdateData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                column: "total_volume_kg",
                value: 1280.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_volume_kg",
                table: "workouts",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "preferred_units",
                table: "users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "metric");

            migrationBuilder.AlterColumn<decimal>(
                name: "weight_kg",
                table: "sets",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_lifted",
                table: "exercise_records",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_weight_kg",
                table: "exercise_records",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "max_volume",
                table: "exercise_records",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "max_total_volume",
                table: "exercise_records",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.UpdateData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999991"),
                columns: new[] { "max_total_volume", "max_volume", "max_weight_kg", "total_lifted" },
                values: new object[] { 4500.0m, 960.0m, 100.0m, 42000.0m });

            migrationBuilder.UpdateData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999992"),
                columns: new[] { "max_total_volume", "max_volume", "max_weight_kg", "total_lifted" },
                values: new object[] { 5200.0m, 1200.0m, 140.0m, 58000.0m });

            migrationBuilder.UpdateData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999993"),
                columns: new[] { "max_total_volume", "max_volume", "max_weight_kg", "total_lifted" },
                values: new object[] { 4800.0m, 1280.0m, 180.0m, 48000.0m });

            migrationBuilder.UpdateData(
                table: "exercise_records",
                keyColumn: "id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999994"),
                columns: new[] { "max_total_volume", "max_volume", "max_weight_kg", "total_lifted" },
                values: new object[] { 2100.0m, 520.0m, 65.0m, 28000.0m });

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888801"),
                column: "weight_kg",
                value: 60.0m);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888802"),
                column: "weight_kg",
                value: 70.0m);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888803"),
                column: "weight_kg",
                value: 80.0m);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888804"),
                column: "weight_kg",
                value: 40.0m);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888805"),
                column: "weight_kg",
                value: 45.0m);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888806"),
                column: "weight_kg",
                value: 10.0m);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888807"),
                column: "weight_kg",
                value: 10.0m);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888808"),
                column: "weight_kg",
                value: 80.0m);

            migrationBuilder.UpdateData(
                table: "sets",
                keyColumn: "id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888809"),
                column: "weight_kg",
                value: 85.0m);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666601"),
                column: "planned_weight",
                value: 60.0m);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666602"),
                column: "planned_weight",
                value: 70.0m);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666603"),
                column: "planned_weight",
                value: 80.0m);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666604"),
                column: "planned_weight",
                value: 40.0m);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666605"),
                column: "planned_weight",
                value: 45.0m);

            migrationBuilder.UpdateData(
                table: "template_sets",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666606"),
                column: "planned_weight",
                value: 50.0m);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "preferred_units",
                value: "metric");

            migrationBuilder.UpdateData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"),
                column: "total_volume_kg",
                value: 1450.50m);

            migrationBuilder.UpdateData(
                table: "workouts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"),
                column: "total_volume_kg",
                value: 1280.00m);
        }
    }
}
