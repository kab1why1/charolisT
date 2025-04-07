using System;

namespace charolis.Models;

public class Guess : User
{
    public string Role { get; set; } = "Guest";

    public Guess() : base() { }

    public override void ShowInfo()
    {
        Console.Write($"Id - {Id}. You'r just a guest, mate");
    }

}

public class GuessRepository : Repository<Guess> { }
