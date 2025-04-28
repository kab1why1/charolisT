using charolis.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charolis.Services.Interfaces
{
    public interface IUserService
    {
        void Create(User user);
        void Update(User user);
        void Delete(int id);
        User? GetById(int id);
        List<User> GetAll();
        List<ValidationResult> ValidateEntity<T>(T entity) where T : class;
        bool IsValid<T>(T entity) where T : class;
    }
}