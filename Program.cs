using Contatos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyHeader",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});
var app = builder.Build();

using var dbContext = new ApplicationDbContext();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAnyHeader");
}

app.UseHttpsRedirection();

app.MapGet("/getall", () =>
{
    return dbContext.Reminder.ToArray();
})
.WithName("getall")
.WithOpenApi();

app.MapPost("/post", async (Reminders reminder) =>
{
    dbContext.Reminder.Add(reminder);
    dbContext.SaveChanges();
    return Results.Created($"/reminder/{reminder.Id}", reminder);

})
.WithName("post")
.WithOpenApi(); ;

app.MapDelete("/delete/{id}", async (int id) =>
{
    var reminder = await dbContext.Reminder.FindAsync(id);
    if (reminder == null)
    {
        return Results.NotFound();
    }

    dbContext.Reminder.Remove(reminder);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("Delete")
.WithOpenApi(); ;

app.Run();






