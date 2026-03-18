using System;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Contracts.Auth;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Infrastructure.Auth
{
    public class AppUserService(ISqlConnectionFactory connectionFactory) : IAppUserService
    {
        public async Task<string> GetRoleAsync(string email)
        {
            var connection = connectionFactory.GetOpenConnection();
            var role = await connection.QuerySingleOrDefaultAsync<string>(
                "SELECT \"Role\" FROM app.\"AppUsers\" WHERE \"Email\" = @Email",
                new { Email = email });

            return role ?? "User";
        }

        public async Task<bool> RegisterAsync(string email, string name, string password)
        {
            var connection = connectionFactory.GetOpenConnection();

            var exists = await connection.QuerySingleOrDefaultAsync<string>(
                "SELECT \"Email\" FROM app.\"AppUsers\" WHERE \"Email\" = @Email",
                new { Email = email });

            if (exists is not null)
                return false;

            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            await connection.ExecuteAsync(
                """
                INSERT INTO app."AppUsers" ("Email", "PasswordHash", "Name", "Role", "CreatedAt")
                VALUES (@Email, @PasswordHash, @Name, 'User', @CreatedAt)
                """,
                new { Email = email, PasswordHash = hash, Name = name, CreatedAt = DateTime.UtcNow });

            return true;
        }

        public async Task<(bool Success, string Role, string Name)> ValidateCredentialsAsync(string email, string password)
        {
            var connection = connectionFactory.GetOpenConnection();

            var user = await connection.QuerySingleOrDefaultAsync<AppUserRow>(
                "SELECT \"Email\", \"PasswordHash\", \"Name\", \"Role\" FROM app.\"AppUsers\" WHERE \"Email\" = @Email",
                new { Email = email });

            if (user is null || string.IsNullOrEmpty(user.PasswordHash))
                return (false, null, null);

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (false, null, null);

            return (true, user.Role, user.Name);
        }

        public async Task EnsureGoogleUserAsync(string email, string name)
        {
            var connection = connectionFactory.GetOpenConnection();

            await connection.ExecuteAsync(
                """
                INSERT INTO app."AppUsers" ("Email", "Name", "Role", "CreatedAt")
                VALUES (@Email, @Name, 'User', @CreatedAt)
                ON CONFLICT ("Email") DO UPDATE SET "Name" = EXCLUDED."Name", "UpdatedAt" = @CreatedAt
                """,
                new { Email = email, Name = name, CreatedAt = DateTime.UtcNow });
        }

        private class AppUserRow
        {
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public string Name { get; set; }
            public string Role { get; set; }
        }
    }
}
