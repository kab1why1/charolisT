// Services/Implementation/OrderService.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using charolis.DAL;
using charolis.Data;
using charolis.Entity;
using charolis.Services.Interfaces;

namespace charolis.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly GenericRepository<Order>     _orderRepo;
        private readonly GenericRepository<OrderItem> _itemRepo;
        private readonly IUserService                 _userService;
        private readonly IProductService              _productService;

        public OrderService(
            GenericRepository<Order> orderRepo,
            GenericRepository<OrderItem> itemRepo,
            IUserService userService,
            IProductService productService)
        {
            _orderRepo      = orderRepo;
            _itemRepo       = itemRepo;
            _userService    = userService;
            _productService = productService;
        }

        public List<Order> GetAll()
        {
            var orders = _orderRepo.GetAll();
            foreach (var o in orders)
            {
                o.User  = _userService.GetById(o.UserId)!;
                o.Items = _itemRepo.GetAll()
                           .Where(i => i.OrderId == o.Id)
                           .ToList();
                foreach (var it in o.Items)
                    it.Product = _productService.GetById(it.ProductId)!;
            }
            return orders;
        }

        public Order? GetById(int id)
        {
            var o = _orderRepo.GetById(id);
            if (o == null) return null;
            o.User  = _userService.GetById(o.UserId)!;
            o.Items = _itemRepo.GetAll().Where(i => i.OrderId == id).ToList();
            foreach (var it in o.Items)
                it.Product = _productService.GetById(it.ProductId)!;
            return o;
        }

        public void Create(int userId, Dictionary<int,int> productQuantities)
        {
            // 1) Перевіряємо базові властивості замовлення
            if (_userService.GetById(userId) == null)
                throw new ValidationException("Оберіть існуючого користувача.");

            if (productQuantities == null || productQuantities.Count == 0)
                throw new ValidationException("Замовлення повинно містити хоча б один товар.");

            // 2) Створюємо та зберігаємо сам Order (щоб отримати Id)
            var order = new Order { UserId = userId };
            ValidateEntity(order);
            _orderRepo.Add(order);

            // 3) Додаємо кожен OrderItem
            decimal total = 0;
            foreach (var kv in productQuantities)
            {
                var prod = _productService.GetById(kv.Key)
                           ?? throw new ValidationException($"Товар з ID={kv.Key} не знайдено.");
                int qty = kv.Value;
                // Валідуємо айтем
                var item = new OrderItem
                {
                    OrderId         = order.Id,
                    ProductId       = prod.Id,
                    Quantity        = qty,
                    PriceAtPurchase = prod.Price
                };
                ValidateEntity(item);
                _itemRepo.Add(item);
                total += prod.Price * qty;
            }

            // 4) Оновлюємо total і зберігаємо order
            order.Total = total;
            _orderRepo.Update(order);
        }

        public void Update(Order order, Dictionary<int,int> productQuantities)
        {
            var existing = _orderRepo.GetById(order.Id)
                         ?? throw new ValidationException("Замовлення не знайдено.");

            if (_userService.GetById(order.UserId) == null)
                throw new ValidationException("Оберіть існуючого користувача.");

            if (productQuantities == null || productQuantities.Count == 0)
                throw new ValidationException("Замовлення повинно містити хоча б один товар.");

            existing.UserId = order.UserId;
            ValidateEntity(existing);
            _orderRepo.Update(existing);

            // Видаляємо старі айтеми
            var oldItems = _itemRepo.GetAll().Where(i => i.OrderId == order.Id).ToList();
            foreach (var oi in oldItems)
                _itemRepo.Delete(oi.Id);

            // Додаємо нові айтеми
            decimal total = 0;
            foreach (var kv in productQuantities)
            {
                var prod = _productService.GetById(kv.Key)
                           ?? throw new ValidationException($"Товар з ID={kv.Key} не знайдено.");
                int qty = kv.Value;
                var item = new OrderItem
                {
                    OrderId         = order.Id,
                    ProductId       = prod.Id,
                    Quantity        = qty,
                    PriceAtPurchase = prod.Price
                };
                ValidateEntity(item);
                _itemRepo.Add(item);
                total += prod.Price * qty;
            }

            existing.Total = total;
            _orderRepo.Update(existing);
        }

        public void Delete(int id)
        {
            var o = _orderRepo.GetById(id)
                    ?? throw new ValidationException("Замовлення не знайдено.");
            // Видаляємо айтеми
            var items = _itemRepo.GetAll().Where(i => i.OrderId == id).ToList();
            foreach (var oi in items)
                _itemRepo.Delete(oi.Id);
            // Видаляємо замовлення
            _orderRepo.Delete(id);
        }

        public List<ValidationResult> ValidateEntity<T>(T entity) where T : class
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, results, true);
            return results;
        }

        public bool IsValid<T>(T entity) where T : class => ValidateEntity(entity).Count == 0;
    }
}
