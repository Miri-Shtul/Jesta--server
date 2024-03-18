using Microsoft.EntityFrameworkCore;

public static class UsersController
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users", async (AppDbContext db) =>
    await db.Users.ToListAsync());

        app.MapGet("/users/{id}", async (int id, AppDbContext db) =>
            await db.Users.FindAsync(id)
                is User user
                    ? Results.Ok(user)
                    : Results.NotFound());

        app.MapPost("/users", async (User user, AppDbContext db) =>
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Created($"/users/{user.Id}", user);
        });

        app.MapPut("/users/{id}", async (int id, User updatedUser, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user is null) return Results.NotFound();
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;
            user.Image = updatedUser.Image;
            // user.Password= updatedUser.Password;
            user.UpdatedAt = new DateTime();
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
        {
            if (await db.Users.FindAsync(id) is User user)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }

            return Results.NotFound();
        });
    }
}
