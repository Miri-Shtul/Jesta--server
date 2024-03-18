
using Microsoft.EntityFrameworkCore;

public static class CategoryController
{
    public static void MapCategoryEndPoints(this WebApplication app)
    {
        app.MapGet("/categories", async (AppDbContext db) =>
        {
            await db.Categories.ToListAsync();
        });
        app.MapPut("/categories", async (AppDbContext db, Category category) =>
        {
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return Results.Created($"/categories/{category.Id}", category);
        });
        app.MapDelete("/categories", async (int id, AppDbContext db) =>
        {
            if (await db.Categories.FindAsync(id) is Category category)
            {
                db.Categories.Remove(category);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        });
    }
}
