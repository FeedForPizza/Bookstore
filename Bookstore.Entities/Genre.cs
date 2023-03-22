using System;
using System.Collections.Generic;

namespace Bookstore.Entities;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
