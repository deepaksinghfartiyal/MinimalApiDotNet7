using System;
using System.Collections.Generic;

namespace MinimalAPI_CRUD_DotNet_7.DB;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? BookAttributeId { get; set; }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual BookAttribute? BookAttribute { get; set; }
}
