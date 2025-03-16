using System;
using System.Collections.Generic;

namespace Project_Group3_SWD.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
