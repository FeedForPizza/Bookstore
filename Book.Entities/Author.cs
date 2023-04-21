using System;
using System.Collections.Generic;

namespace Bookstore.Entities;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Nationality { get; set; }

    public byte[]? Photo { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
