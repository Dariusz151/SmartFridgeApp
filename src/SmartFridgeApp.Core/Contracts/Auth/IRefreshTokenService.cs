using System.Threading.Tasks;

namespace SmartFridgeApp.Core.Contracts.Auth
{
    public interface IRefreshTokenService
    {
        /// <summary>
        /// Creates a new refresh token for the user, stores the hash, and returns the raw token string.
        /// </summary>
        Task<string> CreateTokenAsync(string email);

        /// <summary>
        /// Validates the raw refresh token. If valid, revokes it and issues a new one (rotation).
        /// Returns (email, newRawToken) on success, or (null, null) if invalid/expired/revoked.
        /// </summary>
        Task<(string Email, string NewToken)?> RotateTokenAsync(string rawToken);

        /// <summary>
        /// Revokes all refresh tokens for the given email.
        /// </summary>
        Task RevokeAllAsync(string email);
    }
}
