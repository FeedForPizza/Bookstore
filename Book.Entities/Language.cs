﻿using System;
using System.Collections.Generic;

namespace Bookstore.Entities;

public partial class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
