using System;
using System.Collections.Generic;

namespace MinimalAPI_CRUD_DotNet_7.DB;

public partial class Employee
{
    public int Id { get; set; }

    public string? EmployeeName { get; set; }

    public int? DepartmentId { get; set; }

    public int? JobId { get; set; }

    public int? CountryId { get; set; }

    public int? ManagerId { get; set; }

    public int? ProjectId { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Department? Department { get; set; }

    public virtual JobTitle? Job { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual Project? Project { get; set; }
}
