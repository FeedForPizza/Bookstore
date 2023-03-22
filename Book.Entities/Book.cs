using System;
using System.Collections.Generic;

namespace Bookstore.Entities;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? CoverImage { get; set; }

    public string Isbn { get; set; } = null!;

    public string? Publisher { get; set; }

    public int? PublishingYear { get; set; }

    public decimal? Price { get; set; }

    public int LanguageId { get; set; }

    public virtual Language Language { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<Author> Authors { get; } = new List<Author>();

    public virtual ICollection<Genre> Genres { get; } = new List<Genre>();
}
