
using Microsoft.EntityFrameworkCore;

public static class JestaController
{
    public static void MapJestaEndPoints(this WebApplication app)
    {
        app.MapGet("/jestas", async (AppDbContext db) =>
  {
      await db.Jesta.ToListAsync();
  });
        app.MapGet("/jestas/{id}", async (int id, AppDbContext db) =>
            await db.Jesta.FindAsync(id)
                is Jesta jesta
                    ? Results.Ok(jesta)
                    : Results.NotFound());

        app.MapGet("/jestas/offeredId/{id}", async (int userId, AppDbContext db) =>
        {
            return await db.Jesta
            .Where(jesta => jesta.OfferedId == userId).ToListAsync();
        });
        app.MapGet("/jestas/offereeId/{id}", async (int userId, AppDbContext db) =>
        {
            return await db.Jesta
            .Where(jesta => jesta.OffereeId == userId).ToListAsync();
        });
        app.MapGet("/jestas/transactionId/{id}", async (int transactionId, AppDbContext db) =>
        {
            return await db.Jesta
            .Where(jesta => jesta.TransactionId == transactionId).ToListAsync();
        });

        app.MapGet("/jestas/status/{status:int}", async (Status status, AppDbContext db) =>
        {
            return await db.Jesta
                .Where(jesta => jesta.Status == status)
                .ToListAsync();
        });

        app.MapPost("/jestas", async (Jesta jesta, AppDbContext db) =>
        {
            db.Jesta.Add(jesta);
            await db.SaveChangesAsync();

            return Results.Created($"/jestas/{jesta.Id}", jesta);
        });

        app.MapPut("/jestas/{id}", async (int id, Jesta updatedJesta, AppDbContext db) =>
        {
            var jesta = await db.Jesta.FindAsync(id);

            if (jesta is null) return Results.NotFound();
            jesta.OfferedId = updatedJesta.OfferedId;
            jesta.OffereeId = updatedJesta.OffereeId;
            jesta.Status = updatedJesta.Status;
            jesta.Details = updatedJesta.Details;
            jesta.UpdatedAt = new DateTime();

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapDelete("/jestas/{id}", async (int id, AppDbContext db) =>
        {
            if (await db.Jesta.FindAsync(id) is Jesta jesta)
            {
                db.Jesta.Remove(jesta);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }

            return Results.NotFound();
        });
    }
}
