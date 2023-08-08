using MinimalAPI_CRUD_DotNet_7.DB;
using MinimalAPI_CRUD_DotNet_7.ViewModal;

namespace MinimalAPI_CRUD_DotNet_7.Minimal_API_Routes
{
    public static class JoinUsingLinqAnd_LambdaExpressionEf
    {
        public static void JoinUsingLinqAndLambda(this WebApplication app)
        {
            #region InnerJoinQuearySyntax
            app.MapGet("InnerJoinQuearySyntax/Employee", async (DbGenricRepositoryPatternContext db) =>
            {
                var data = from e in db.Employees
                           join d in db.Departments on e.DepartmentId equals d.Id
                           join j in db.JobTitles on e.JobId equals j.Id
                           join c in db.Countries on e.CountryId equals c.Id
                           join m in db.Managers on e.ManagerId equals m.Id
                           join p in db.Projects on e.ProjectId equals p.Id
                           select new
                           {
                               e.Id,
                               e.EmployeeName,
                               DepartmentName = d.DepartmentName,
                               JobTitllename = j.JobTitleName,
                               countryName = c.CountryName,
                               managerName = m.ManagerName,
                               Projectname = p.ProjectName
                           };
                if (data == null) return Results.NoContent();
                return Results.Ok(data);
            }).RequireAuthorization();
            #endregion

            #region InnerJoinLambdaExpression
            app.MapGet("InnerJoinLambdaExpression/Employee", (DbGenricRepositoryPatternContext db) =>
            {
                //var results11 = db.Employees.FromSqlRaw("SELECT e.employee_id,e.employee_name,e.country_id as CountryId,d.department_name,jt.job_title_name,c.country_name,m.manager_name,p.project_name FROM employees e JOIN department d ON e.department_id = d.department_id JOIN job_title jt ON e.job_id = jt.job_id JOIN country c ON e.country_id = c.country_id JOIN manager m ON e.manager_id = m.manager_id JOIN project p ON e.project_id = p.project_id\r\n").ToList();
                var query = db.Employees.Join(db.Departments, e => e.DepartmentId, d => d.Id, (emp, dep) => new
                {
                    employees_A = emp,
                    department_A = dep
                })
                  .Join(db.JobTitles, ed => ed.employees_A.JobId, j => j.Id, (ed, j) => new
                  {
                      ed.employees_A,
                      ed.department_A,
                      jobTitle_A = j
                  })
                  .Join(db.Countries, ec => ec.employees_A.CountryId, c => c.Id, (e_c, coun) => new
                  {
                      e_c.employees_A,
                      e_c.department_A,
                      e_c.jobTitle_A,
                      Country_A = coun
                  })
                  .Join(db.Managers, em => em.employees_A.ManagerId, m => m.Id, (em, m) => new
                  {
                      em.employees_A,
                      em.department_A,
                      em.jobTitle_A,
                      em.Country_A,
                      Manager_A = m
                  })
                  .Join(db.Projects, ep => ep.employees_A.ProjectId, p => p.Id, (ep, p) => new
                  {
                      ep.employees_A,
                      ep.department_A,
                      ep.jobTitle_A,
                      ep.Country_A,
                      ep.Manager_A,
                      Project_A = p
                  });
                var results = query.ToList();
                var dtoresult = results.Select(data => new EmployeeJoinResultDto
                {
                    EmployeeId = data.employees_A.Id,
                    EmployeeName = data.employees_A?.EmployeeName ?? "No data",
                    JobTitleName = data.jobTitle_A?.JobTitleName ?? "No data",
                    DepartmentName = data.department_A?.DepartmentName ?? "No data",
                    CountryName = data.Country_A?.CountryName ?? "No data",
                    ManagerName = data.Manager_A?.ManagerName ?? "No data",
                    ProjectName = data.Project_A?.ProjectName ?? "No data"
                }).ToList();
                if (dtoresult == null) Results.NoContent();
                return Results.Ok(dtoresult);
            }).RequireAuthorization();
            #endregion
        }
    }
}
