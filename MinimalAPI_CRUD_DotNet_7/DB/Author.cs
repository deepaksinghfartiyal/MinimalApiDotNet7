using System;
using System.Collections.Generic;

namespace MinimalAPI_CRUD_DotNet_7.DB;

public partial class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? GenreId { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual Genre? Genre { get; set; }
}
