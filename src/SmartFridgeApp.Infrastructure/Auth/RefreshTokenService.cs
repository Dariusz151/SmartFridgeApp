using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Contracts.Auth;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Infrastructure.Auth
{
    public class RefreshTokenService(ISqlConnectionFactory connectionFactory) : IRefreshTokenService
    {
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromDays(7);

        public async Task<string> CreateTokenAsync(string email)
        {
            var rawToken = GenerateToken();
            var hash = HashToken(rawToken);

            var connection = connectionFactory.GetOpenConnection();

            await connection.ExecuteAsync(
                """
                INSERT INTO app."RefreshTokens" ("Email", "TokenHash", "ExpiresAt", "CreatedAt")
                VALUES (@Email, @TokenHash, @ExpiresAt, @CreatedAt)
                """,
                new
                {
                    Email = email,
                    TokenHash = hash,
                    ExpiresAt = DateTime.UtcNow.Add(TokenLifetime),
                    CreatedAt = DateTime.UtcNow
                });

            return rawToken;
        }

        public async Task<(string Email, string NewToken)?> RotateTokenAsync(string rawToken)
        {
            var hash = HashToken(rawToken);
            var connection = connectionFactory.GetOpenConnection();

            var row = await connection.QuerySingleOrDefaultAsync<RefreshTokenRow>(
                """
                SELECT "Id", "Email", "ExpiresAt", "RevokedAt"
                FROM app."RefreshTokens"
                WHERE "TokenHash" = @TokenHash
                """,
                new { TokenHash = hash });

            if (row is null || row.RevokedAt.HasValue || row.ExpiresAt < DateTime.UtcNow)
                return null;

            // Revoke the used token
            await connection.ExecuteAsync(
                """
                UPDATE app."RefreshTokens" SET "RevokedAt" = @Now WHERE "Id" = @Id
                """,
                new { Now = DateTime.UtcNow, row.Id });

            // Issue a new one
            var newToken = await CreateTokenAsync(row.Email);
            return (row.Email, newToken);
        }

        public async Task RevokeAllAsync(string email)
        {
            var connection = connectionFactory.GetOpenConnection();

            await connection.ExecuteAsync(
                """
                UPDATE app."RefreshTokens"
                SET "RevokedAt" = @Now
                WHERE "Email" = @Email AND "RevokedAt" IS NULL
                """,
                new { Now = DateTime.UtcNow, Email = email });
        }

        private static string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        private static string HashToken(string rawToken)
        {
            var bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToHexStringLower(bytes);
        }

        private class RefreshTokenRow
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public DateTime ExpiresAt { get; set; }
            public DateTime? RevokedAt { get; set; }
        }
    }
}
