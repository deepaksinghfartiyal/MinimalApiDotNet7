using System;
using System.Collections.Generic;

namespace MinimalAPI_CRUD_DotNet_7.DB;

public partial class Department
{
    public int Id { get; set; }

    public string? DepartmentName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
