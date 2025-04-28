using charolis.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charolis.Services.Interfaces
{
    public interface IProductService
    {
        void Create(Product product);
        void Update(Product product);
        void Delete(int id);
        Product? GetById(int id);
        List<Product> GetAll();
        List<Product> GetActive(); // ✅ ДОДАТИ ЦЕ
        List<ValidationResult> ValidateEntity<T>(T entity) where T : class;
        bool IsValid<T>(T entity) where T : class;
    }
}