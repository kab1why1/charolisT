using System;

namespace charolis.Models;

public class Product : BaseEntity
{
    private string name;
    private string desc;
    private decimal price;
    private bool availible;

    public string Name {get => name; set => name = value; }
    public string Desc {get => desc; set => desc = value; }
    public decimal Price {get => price; set => price = value; }
    public bool Awailible {get => availible; set => availible = value; }

    public Product() { }

    public Product(string name, string desc, decimal price, bool availible)
    { 
        this.name = name;
        this.desc = desc;
        this.price = price;
        this.availible = availible;
    }

    public override string ToString() => $"ID - {Id}, Name - {Name}, Description - {Desc}, Price - {Price}, Availible - {Awailible}";

}

public class ProductRepository : Repository<Product> { }
