using Microsoft.Data.SqlClient;
using MinimalAPI_CRUD_DotNet_7.ViewModal;
using System.Data;
using MinimalAPI_CRUD_DotNet_7.DB;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace MinimalAPI_CRUD_DotNet_7.Minimal_API_Routes
{
    public class ApiConfigurationForSpCall
    {
        private readonly DbGenricRepositoryPatternContext _dbContext;

        public ApiConfigurationForSpCall(DbGenricRepositoryPatternContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string GetConnectionString()
        {
            return _dbContext.Database.GetConnectionString(); 
        }

 
        //MapGet should be used with an instance of IEndpointRouteBuilder.
        public void ConfigureSpEndpoints(IEndpointRouteBuilder app)
        {
            #region Endpoint_to_call_storedprocedure_ADO_Net
            app.MapGet("/api/employees_ado", () =>
            {
                List<EmployeeJoinResultDto> results = new List<EmployeeJoinResultDto>();

                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SpGetEmployeeJoinResults", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeJoinResultDto dto = new EmployeeJoinResultDto
                                {
                                    EmployeeId = Convert.ToInt32(reader["Id"]),
                                    EmployeeName = reader["EmployeeName"].ToString(),
                                    DepartmentName = reader["DepartmentName"].ToString(),
                                    JobTitleName = reader["JobTitleName"].ToString(),
                                    CountryName = reader["CountryName"].ToString(),
                                    ManagerName = reader["ManagerName"].ToString(),
                                    ProjectName = reader["ProjectName"].ToString()
                                };

                                results.Add(dto);
                            }
                        }
                    }
                }
                return Results.Ok(results);
            });//.RequireAuthorization();
            #endregion

            #region Endpoint_to_call_storedprocedure_ADO_Net
            app.MapGet("/api/employees/sp_ef", () =>
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    // Execute the stored procedure using Dapper and map the results to DTO
                    var employees = connection.Query<EmployeeJoinResultDto>("SpGetEmployeeJoinResults", commandType: CommandType.StoredProcedure).ToList();
                    return Results.Ok(employees);
                }
            });
            #endregion

            #region Endpoint_to_call_storedprocedure_Using
            app.MapGet("/api/employees/s.p dapper", () =>
            {
                var employees = _dbContext.Employees.FromSqlRaw<Employee>("EXEC SpGetEmployeeJoinResults").ToList();
                // Explicitly load the related entities for each employee
                foreach (var employee in employees)
                {
                    var entry = _dbContext.Entry(employee);

                    entry.Reference(e => e.Department).Load();
                    entry.Reference(e => e.Job).Load();
                    entry.Reference(e => e.Country).Load();
                    entry.Reference(e => e.Manager).Load();
                    entry.Reference(e => e.Project).Load();
                }
                var results = employees.Select(e => new EmployeeJoinResultDto
                {
                    EmployeeId = e.Id,
                    EmployeeName = e.EmployeeName,
                    DepartmentName = e.Department?.DepartmentName,
                    JobTitleName = e.Job?.JobTitleName,
                    CountryName = e.Country?.CountryName,
                    ManagerName = e.Manager?.ManagerName,
                    ProjectName = e.Project?.ProjectName
                }).ToList();
                return Results.Ok(results);
            });
            #endregion
        }
        

    }
}
