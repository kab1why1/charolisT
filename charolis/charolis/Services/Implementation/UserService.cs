using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using charolis.DAL;
using charolis.Data;
using charolis.Entity;
using charolis.Services.Interfaces;

namespace charolis.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly GenericRepository<User> _repo;

        public UserService(GenericRepository<User> repo)
        {
            _repo = repo;
        }

        public void Create(User user)
        {
            ValidateEntity(user);

            if (_repo.GetAll().Any(u => u.Username == user.Username))
                throw new ValidationException("Користувач з таким логіном вже існує.");

            _repo.Add(user);
        }

        public void Update(User user)
        {
            ValidateEntity(user);

            var existing = _repo.GetById(user.Id);
            if (existing == null)
                throw new ValidationException("Користувача не знайдено.");

            // Захист від зміни чужого облікового запису можна додати тут

            _repo.Update(user);
        }

        public void Delete(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
                throw new ValidationException("Користувача не знайдено.");

            _repo.Delete(id);
        }

        public User? GetById(int id) => _repo.GetById(id);

        public List<User> GetAll() => _repo.GetAll();

        public List<ValidationResult> ValidateEntity<T>(T entity) where T : class
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, results, validateAllProperties: true);
            return results;
        }

        public bool IsValid<T>(T entity) where T : class => ValidateEntity(entity).Count == 0;
    }
}