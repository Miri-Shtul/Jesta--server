
using Microsoft.EntityFrameworkCore;

public static class TransactionController
{
    public static void MapTransactionEndPoints(this WebApplication app)
    {
        app.MapGet("/transactions", async (AppDbContext db) =>
            await db.Transactions.ToListAsync());

        app.MapGet("/transactions/{id}", async (int id, AppDbContext db) =>
            await db.Transactions.FindAsync(id)
                is Transaction transactions
                    ? Results.Ok(transactions)
                    : Results.NotFound());

        app.MapGet("/transactions/user/{id}", async (int userId, AppDbContext db) =>
        {
            return await db.Transactions
            .Where(transaction => transaction.UserId == userId).ToListAsync();
        });

        app.MapGet("/transactions/category/{id}", async (int categoryId, AppDbContext db) =>
        {
            return await db.Transactions
                .Where(transaction => transaction.CategoryIds.Contains(categoryId))
                .ToListAsync();
        });

        app.MapPost("/transactions", async (Transaction transaction, AppDbContext db) =>
        {
            db.Transactions.Add(transaction);
            await db.SaveChangesAsync();

            return Results.Created($"/transactions/{transaction.Id}", transaction);
        });

        app.MapPut("/transactions/{id}", async (int id, Transaction updatedtransaction, AppDbContext db) =>
        {
            var transaction = await db.Transactions.FindAsync(id);

            if (transaction is null) return Results.NotFound();
            transaction.Name = updatedtransaction.Name;
            transaction.CategoryIds = updatedtransaction.CategoryIds;
            transaction.Description = updatedtransaction.Description;
            transaction.Location = updatedtransaction.Location;
            transaction.Image = updatedtransaction.Image;
            transaction.Updatedat = updatedtransaction.Updatedat;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapDelete("/transactions/{id}", async (int id, AppDbContext db) =>
        {
            if (await db.Transactions.FindAsync(id) is Transaction transaction)
            {
                db.Transactions.Remove(transaction);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }

            return Results.NotFound();
        });
    }
}
