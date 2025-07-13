using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LucasRT.RavenDB.Demo.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "beverage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false),
                    BeverageType = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Variety = table.Column<string>(type: "text", nullable: true),
                    Producer = table.Column<string>(type: "text", nullable: true),
                    Vintage = table.Column<int>(type: "integer", nullable: true),
                    IBU = table.Column<double>(type: "double precision", nullable: true),
                    AlcoholPercentage = table.Column<double>(type: "double precision", nullable: false),
                    Country = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beverage", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "beverage");
        }
    }
}
