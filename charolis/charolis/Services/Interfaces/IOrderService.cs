// Services/Interfaces/IOrderService.cs
using charolis.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charolis.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAll();
        Order? GetById(int id);
        void Create(int userId, Dictionary<int,int> productQuantities);
        void Update(Order order, Dictionary<int,int> productQuantities);
        void Delete(int id);

        List<ValidationResult> ValidateEntity<T>(T entity) where T : class;
        bool IsValid<T>(T entity) where T : class;
    }
}