using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI_CRUD_DotNet_7.DB;
//using MinimalAPI_CRUD_DotNet_7.DB;
using test.Models;

namespace MinimalAPI_CRUD_DotNet_7.Minimal_API_Routes
{
    public static class Employee1Crud_MinimalAPI
    {
        public static void Employee1Service(this WebApplication app)
        {
            app.MapGet("/GetAll", async (DbGenricRepositoryPatternContext dbContext) =>
            {
                var employees = await dbContext.TblEmployees.ToListAsync();
                if (employees == null)
                {
                    return Results.NoContent();
                }
                return Results.Ok(employees);
            }).RequireAuthorization();

            app.MapGet("GetById/{id}", async (int id, DbGenricRepositoryPatternContext dbcontext) =>
            {
                TblEmployee emp = await dbcontext.TblEmployees.Where(e => e.EmployeeId == id).FirstOrDefaultAsync();
                if (id == 0 && id < 0)
                {
                    return Results.NoContent();
                }
                return Results.Ok(emp);
            }).RequireAuthorization();

            app.MapPost("PostIteam", async ([FromBody] TblEmployee emp, [FromServices] DbGenricRepositoryPatternContext dbcontext) =>
            {
               await dbcontext.TblEmployees.AddAsync(emp);
               await dbcontext.SaveChangesAsync();
                return Results.Ok();
            });

            app.MapPut("updateIteam/{id}", async (int id, TblEmployee emp, DbGenricRepositoryPatternContext dbcontext) =>
            {
                var data = await dbcontext.TblEmployees.FindAsync(id);
                if (data == null)
                {
                    return Results.NoContent();
                }
                data.Name = emp.Name;
                data.Salary = emp.Salary;
                data.Gender = emp.Gender;
                data.Dept = emp.Dept;

                await dbcontext.SaveChangesAsync();
                return Results.Ok(data);
            });

            app.MapDelete("Delete/{id}", async (int id, DbGenricRepositoryPatternContext dbContext) =>
            {
                var iteam = await dbContext.TblEmployees.FindAsync(id);
                if (iteam == null)
                {
                    return Results.NoContent();
                }
                dbContext.TblEmployees.Remove(iteam);
                await dbContext.SaveChangesAsync();
                return Results.Ok(iteam);
            });
        }
    }
}
