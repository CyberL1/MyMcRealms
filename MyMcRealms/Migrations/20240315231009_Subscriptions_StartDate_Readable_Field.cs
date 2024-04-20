using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMcRealms.Migrations
{
    /// <inheritdoc />
    public partial class Subscriptions_StartDate_Readable_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Subscriptions\" ALTER COLUMN \"StartDate\" TYPE timestamptz USING \"StartDate\"::timestamptz");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Subscriptions\" ALTER COLUMN \"StartDate\" TYPE text USING \"StartDate\"::text");
        }
    }
}
