using System.Linq;
using charolis.Models;

namespace charolis.Services
{
    public class AuthService : IAuthService
    {
        private readonly RegUserRepository _users;
        public AuthService(RegUserRepository users) => _users = users;

        public void Register(RegUser user, string password)
        {
            // Генеруємо хеш пароля
            user.PasswordHash = PasswordHelper.Hash(password);
            _users.Add(user);
        }

        public RegUser? ValidateUser(string email, string password)
        {
            var user = _users.GetAll().FirstOrDefault(u => u.Email == email);
            if (user == null) return null;

            // Передаємо збережений хеш і пароль у правильному порядку
            return PasswordHelper.Check(user.PasswordHash, password)
                ? user 
                : null;
        }
    }
}
