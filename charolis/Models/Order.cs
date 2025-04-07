using System;
namespace charolis.Models;

public class Order : BaseEntity
{   
    private RegUser customer;
    private List<Product> products;
    private decimal totalPrice;
    private int productCount;


    public RegUser Customer {get => customer; set => customer = value; }
    public List<Product> Products {get => products; set => products = value; }
    public decimal TotalPrice {get => totalPrice; set => totalPrice = value; }
    public int ProductCount {get => productCount; set => productCount = value; }

    public Order()
    {
        products = new List<Product>();
    }

    public Order(RegUser customer, List<Product> products)
    {   
        this.customer = customer;
        this.products = products;
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        productCount = products.Count;
        totalPrice = 0;
        foreach (var product in products)
        {
            totalPrice += product.Price;
        }
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
        foreach (var item in products){
            Console.Write($"Name - {customer.Name}, Item - {item.Name}, Price - {item.Price} \n");
        }
    }

}
public class OrdersRepository : Repository<Order> { }