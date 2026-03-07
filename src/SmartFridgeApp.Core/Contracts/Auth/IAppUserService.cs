using System.Threading.Tasks;

namespace SmartFridgeApp.Core.Contracts.Auth
{
    public interface IAppUserService
    {
        /// <summary>
        /// Returns the role for the given email, or "User" if not found.
        /// </summary>
        Task<string> GetRoleAsync(string email);

        /// <summary>
        /// Registers a new user with email and bcrypt-hashed password.
        /// Returns false if the email is already taken.
        /// </summary>
        Task<bool> RegisterAsync(string email, string name, string password);

        /// <summary>
        /// Validates email + password. Returns (success, role, name).
        /// </summary>
        Task<(bool Success, string Role, string Name)> ValidateCredentialsAsync(string email, string password);

        /// <summary>
        /// Ensures a Google-authenticated user exists in AppUsers (upserts name).
        /// </summary>
        Task EnsureGoogleUserAsync(string email, string name);
    }
}
