using System;

namespace charolis.Models;

public abstract class User : BaseEntity
{
    private string name;
    private string email;
    private string password;
    private string phone;
    private string address;

    public string Name {get => name; set => name = value; }
    public string Email {get => email; set => email = value; }
    public string Password {get => password; set => password = value; }
    public string Phone {get => phone; set => phone = value; }
    public string Address {get => address; set => address = value; }

    public User() { }

    public User(string name, string emeil, string password, string phone, string address)
    {
        this.name = name;
        this.email = emeil;
        this.password = password;
        this.phone = phone;
        this.address = address;
    }
    
    public abstract void ShowInfo();
}
