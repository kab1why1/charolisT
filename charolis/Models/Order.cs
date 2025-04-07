using System;
using System.Collections.Generic;
using System.Linq;

namespace charolis.Models
{
    public class Order : BaseEntity
    {
        private RegUser? customer;
        private List<Product> products = new List<Product>();

        public RegUser? Customer   { get => customer; set => customer = value; }
        public List<Product> Products { get => products; set => products = value; }
        public decimal TotalPrice  { get; set; }
        public int ProductCount    { get; set; }

        public Order() { }

        public Order(RegUser customer, List<Product> products)
        {
            this.customer = customer;
            this.products = products;
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            ProductCount = products.Count;
            TotalPrice   = products.Sum(p => p.Price);
        }

        public void Add(Product product)
        {
            products.Add(product);
            CalculateTotal();
        }

        public void Remove(Product product)
        {
            products.Remove(product);
            CalculateTotal();
        }

        public void ShowItems()
        {
            foreach (var item in products)
                Console.Write($"Name - {customer?.Name}, Item - {item.Name}, Price - {item.Price}\n");
        }
    }

    public class OrdersRepository : Repository<Order> { }
}
