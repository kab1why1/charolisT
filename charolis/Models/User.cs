using charolis.Interfaces;

namespace charolis.Models
{
    public abstract class User : BaseEntity
    {
        private string name = "";
        private string email = "";
        private string phone = "";
        private string address = "";

        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Address { get => address; set => address = value; }

        // Хеш пароля, ініціалізуємо пустим рядком
        public string PasswordHash { get; set; } = "";

        protected User() { }

        protected User(string name, string email, string passwordHash, string phone, string address)
        {
            this.name         = name;
            this.email        = email;
            this.PasswordHash = passwordHash;
            this.phone        = phone;
            this.address      = address;
        }

        public abstract void ShowInfo();
    }
}
