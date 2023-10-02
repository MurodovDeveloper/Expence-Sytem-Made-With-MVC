using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Expence_Sytem_Application_MVC.DataAcces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Expence_Sytem_Application_MVC.Models;
public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Column(TypeName = "varchar(50)")]
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    [Column(TypeName = "varchar(5)")]
    public string Icon { get; set; } = "";

    [Column(TypeName = "varchar(10)")]
    public string Type { get; set; } = "Expense";

    [NotMapped]
    public string? TitleWithIcon
    {
        get
        {
            return this.Icon + " " + this.Title;
        }
    }
}


public static class CategoryEndpoints
{
	public static void MapCategoryEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Category").WithTags(nameof(Category));

        group.MapGet("/", async (ExpenceDbContext db) =>
        {
            return await db.Categories.ToListAsync();
        })
        .WithName("GetAllCategories")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Category>, NotFound>> (int categoryid, ExpenceDbContext db) =>
        {
            return await db.Categories.AsNoTracking()
                .FirstOrDefaultAsync(model => model.CategoryId == categoryid)
                is Category model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCategoryById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int categoryid, Category category, ExpenceDbContext db) =>
        {
            var affected = await db.Categories
                .Where(model => model.CategoryId == categoryid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.CategoryId, category.CategoryId)
                  .SetProperty(m => m.Title, category.Title)
                  .SetProperty(m => m.Icon, category.Icon)
                  .SetProperty(m => m.Type, category.Type)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCategory")
        .WithOpenApi();

        group.MapPost("/", async (Category category, ExpenceDbContext db) =>
        {
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Category/{category.CategoryId}",category);
        })
        .WithName("CreateCategory")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int categoryid, ExpenceDbContext db) =>
        {
            var affected = await db.Categories
                .Where(model => model.CategoryId == categoryid)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCategory")
        .WithOpenApi();
    }
}