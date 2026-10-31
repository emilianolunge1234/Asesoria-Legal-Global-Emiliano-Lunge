using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace AsesoriaLegal
{
    public class AppUser
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegisteredAtUtc { get; set; }
    }

    public static class UserStore
    {
        private static readonly ConcurrentDictionary<string, AppUser> Users =
            new ConcurrentDictionary<string, AppUser>(StringComparer.OrdinalIgnoreCase);

        public static bool UsernameExists(string username)
        {
            return !string.IsNullOrWhiteSpace(username) && Users.ContainsKey(username.Trim());
        }

        public static bool EmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            string normalizada = email.Trim();
            foreach (AppUser usuario in Users.Values)
            {
                if (string.Equals(usuario.Email, normalizada, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool TryRegister(string email, string username, int age, string fullName,
            string password, out AppUser user, out string error)
        {
            user = null;
            error = null;

            string claveUsuario = (username ?? string.Empty).Trim();
            string claveEmail = (email ?? string.Empty).Trim();

            if (claveUsuario.Length == 0 || claveEmail.Length == 0)
            {
                error = "Los datos del usuario están incompletos.";
                return false;
            }

            if (UsernameExists(claveUsuario))
            {
                error = "El nombre de usuario ya está en uso. Elija otro.";
                return false;
            }

            if (EmailExists(claveEmail))
            {
                error = "El correo electrónico ya está registrado.";
                return false;
            }

            AppUser nuevo = new AppUser
            {
                Email = claveEmail,
                Username = claveUsuario,
                Age = age,
                FullName = (fullName ?? string.Empty).Trim(),
                PasswordHash = HashPassword(password),
                RegisteredAtUtc = DateTime.UtcNow
            };

            if (!Users.TryAdd(claveUsuario, nuevo))
            {
                error = "El nombre de usuario ya está en uso. Elija otro.";
                return false;
            }

            user = nuevo;
            return true;
        }

        public static AppUser Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || password == null)
            {
                return null;
            }

            AppUser usuario;
            if (!Users.TryGetValue(username.Trim(), out usuario))
            {
                return null;
            }

            string hashIngresado = HashPassword(password);
            return string.Equals(usuario.PasswordHash, hashIngresado, StringComparison.Ordinal)
                ? usuario
                : null;
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
                StringBuilder sb = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
