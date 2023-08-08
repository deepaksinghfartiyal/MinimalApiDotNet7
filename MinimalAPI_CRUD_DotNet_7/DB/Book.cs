using System;
using System.Collections.Generic;

namespace MinimalAPI_CRUD_DotNet_7.DB;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int? AuthorId { get; set; }

    public int? PublisherId { get; set; }

    public int? BookDetailId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual BookDetail? BookDetail { get; set; }

    public virtual Publisher? Publisher { get; set; }
}
