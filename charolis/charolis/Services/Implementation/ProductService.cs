using charolis.Entity;
using charolis.DAL;
using charolis.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using charolis.Data;

namespace charolis.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly GenericRepository<Product> _repository;

        public ProductService(GenericRepository<Product> repository)
        {
            _repository = repository;
        }

        public void Create(Product product)
        {
            ValidateProduct(product);

            if (_repository.GetAll().Any(p => p.Name == product.Name))
            {
                throw new ValidationException("Продукт із таким іменем вже існує.");
            }

            _repository.Add(product);
        }

        public void Update(Product product)
        {
            ValidateProduct(product);

            var existing = _repository.GetById(product.Id);
            if (existing == null)
            {
                throw new ValidationException("Продукт не знайдено.");
            }

            _repository.Update(product);
        }

        public void Delete(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                throw new ValidationException("Продукт не знайдено.");
            }

            _repository.Delete(id);
        }

        public Product? GetById(int id) => _repository.GetById(id);

        public List<Product> GetAll() => _repository.GetAll();

        public List<Product> GetActive() // ✅ ДОДАНО
        {
            return _repository.GetAll()
                .Where(p => p.IsActive) // Припускаємо, що у Product є поле bool IsActive
                .ToList();
        }

        public List<ValidationResult> ValidateEntity<T>(T entity) where T : class
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, results, validateAllProperties: true);
            return results;
        }

        public bool IsValid<T>(T entity) where T : class => ValidateEntity(entity).Count == 0;

        private void ValidateProduct(Product product)
        {
            var validationResults = ValidateEntity(product);
            if (validationResults.Any())
            {
                var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ValidationException("Помилка валідації: " + errors);
            }
        }
    }
}
