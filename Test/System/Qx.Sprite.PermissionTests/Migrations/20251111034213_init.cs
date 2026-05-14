using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Qx.Sprite.PermissionTests.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "auth_pages",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    app_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    redirect = table.Column<string>(type: "text", nullable: false),
                    meta = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    tenant_id = table.Column<string>(type: "text", nullable: true),
                    creation_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    creator = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    last_modification_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_modifier = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleter = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    deletion_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_pages", x => x.id);
                    table.ForeignKey(
                        name: "fk_auth_pages_auth_pages_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "dbo",
                        principalTable: "auth_pages",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "auth_roles",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    app_id = table.Column<string>(type: "text", nullable: false),
                    tenant_id = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    creation_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    creator = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    last_modification_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_modifier = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleter = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    deletion_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "auth_users",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tenant_id = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    mobile = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    creation_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    creator = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    last_modification_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_modifier = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleter = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    deletion_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "auth_end_points",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    page_id = table.Column<int>(type: "integer", nullable: true),
                    @namespace = table.Column<string>(name: "namespace", type: "text", nullable: false),
                    controller = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_end_points", x => x.id);
                    table.ForeignKey(
                        name: "fk_auth_end_points_pages_page_id",
                        column: x => x.page_id,
                        principalSchema: "dbo",
                        principalTable: "auth_pages",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "auth_operations",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    page_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_operations", x => x.id);
                    table.ForeignKey(
                        name: "fk_auth_operations_pages_page_id",
                        column: x => x.page_id,
                        principalSchema: "dbo",
                        principalTable: "auth_pages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auth_permissions",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    page_id = table.Column<int>(type: "integer", nullable: false),
                    action_permission = table.Column<string>(type: "text", nullable: false),
                    end_points = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_auth_permissions_auth_pages_page_id",
                        column: x => x.page_id,
                        principalSchema: "dbo",
                        principalTable: "auth_pages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_auth_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "dbo",
                        principalTable: "auth_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auth_role_user",
                schema: "dbo",
                columns: table => new
                {
                    roles_id = table.Column<int>(type: "integer", nullable: false),
                    users_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_role_user", x => new { x.roles_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_auth_role_user_auth_roles_roles_id",
                        column: x => x.roles_id,
                        principalSchema: "dbo",
                        principalTable: "auth_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_auth_role_user_auth_users_users_id",
                        column: x => x.users_id,
                        principalSchema: "dbo",
                        principalTable: "auth_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_auth_end_points_page_id",
                schema: "dbo",
                table: "auth_end_points",
                column: "page_id");

            migrationBuilder.CreateIndex(
                name: "ix_auth_operations_page_id",
                schema: "dbo",
                table: "auth_operations",
                column: "page_id");

            migrationBuilder.CreateIndex(
                name: "ix_auth_pages_parent_id",
                schema: "dbo",
                table: "auth_pages",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_auth_permissions_page_id",
                schema: "dbo",
                table: "auth_permissions",
                column: "page_id");

            migrationBuilder.CreateIndex(
                name: "ix_auth_permissions_role_id",
                schema: "dbo",
                table: "auth_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_auth_role_user_users_id",
                schema: "dbo",
                table: "auth_role_user",
                column: "users_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_end_points",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "auth_operations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "auth_permissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "auth_role_user",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "auth_pages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "auth_roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "auth_users",
                schema: "dbo");
        }
    }
}
