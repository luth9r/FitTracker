using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSeederData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$20pVjNtw/EknLSvVjig.z.7aFDUPZhUajZoFvr2lncrVgR2/kNJMW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "password_hash",
                value: "$2a$11$X5wFuQE5cCcYKfZ1EE.IbeQQfFhVxR4rL8CxKgE8X9Y.wU3jZ9r4C");
        }
    }
}
