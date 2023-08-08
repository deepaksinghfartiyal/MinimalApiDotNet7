using System;
using System.Collections.Generic;

namespace MinimalAPI_CRUD_DotNet_7.DB;

public partial class TblEmployee
{
    public int EmployeeId { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public int? Salary { get; set; }

    public string? Dept { get; set; }
}
