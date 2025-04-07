using System;
using charolis.Interfaces;

namespace charolis.Models;

public class BaseEntity : IEntity
{
    public int Id { get; set; }
    private static int IdCount = 0;

    public BaseEntity() {
        Id = IdCount++;
    }
}
