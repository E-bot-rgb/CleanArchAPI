using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchAPI.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}