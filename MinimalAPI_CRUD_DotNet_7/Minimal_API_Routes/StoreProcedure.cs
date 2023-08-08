using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MinimalAPI_CRUD_DotNet_7.DB;
using MinimalAPI_CRUD_DotNet_7.ViewModal;
using System.Data;

namespace MinimalAPI_CRUD_DotNet_7.Minimal_API_Routes
{
    public static class StoreProcedure
    {
      public static List<EmployeeJoinResultDto> GetEmployeeJoinResults_EfCore(this DbGenricRepositoryPatternContext dbContext)
        {
            var employees = dbContext.Employees.FromSqlRaw<Employee>("EXEC SpGetEmployeeJoinResults").ToList();


            // Explicitly load the related entities for each employee
            foreach (var employee in employees)
            {
                var entry = dbContext.Entry(employee);

                entry.Reference(e => e.Department).Load();
                entry.Reference(e => e.Job).Load();
                entry.Reference(e => e.Country).Load();
                entry.Reference(e => e.Manager).Load();
                entry.Reference(e => e.Project).Load();

                //or
                //dbContext.Entry(employee)
                //   .Reference(d => d.Department)
                //   .Load();

                //dbContext.Entry(employee)
                //    .Reference(e => e.Department)
                //    .Load();

                //dbContext.Entry(employee)
                //    .Reference(e => e.Job)
                //    .Load();

                //dbContext.Entry(employee)
                //    .Reference(e => e.Country)
                //    .Load();

                //dbContext.Entry(employee)
                //    .Reference(e => e.Manager)
                //    .Load();

                //dbContext.Entry(employee)
                //    .Reference(e => e.Project)
                //    .Load();
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

            return results;
        }

      public static List<EmployeeJoinResultDto> GetEmployeeJoinResults_UsingStoreprocedure_Dapper()
        {
            using (var connection = new SqlConnection("YourConnectionStringHere"))
            {
                connection.Open();

                // Execute the stored procedure using Dapper and map the results to DTO
                var employees = connection.Query<EmployeeJoinResultDto>("SpGetEmployeeJoinResults", commandType: CommandType.StoredProcedure).ToList();

                return employees;
            }
        }

      public static List<EmployeeJoinResultDto> GetEmployeeJoinResults()
        {
            //Database.c

            List<EmployeeJoinResultDto> results = new List<EmployeeJoinResultDto>();

            using (SqlConnection connection = new SqlConnection(""))
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

            return results;
        }

    }
}
