using BasicTaxFreeAllowanceApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BTFARepository>();

var app = builder.Build();

app.MapGet("/btfa", ([FromServices] BTFARepository repo) =>
{
    return repo.GetAll();
});

app.MapGet("/btfa/{id}", ([FromServices] BTFARepository repo, string id) =>
{
    var record = repo.GetById(id);
    return record is not null ? Results.Ok(record) : Results.NotFound();
});

app.MapPost("/btfa", ([FromServices] BTFARepository repo, BTFARecord record) =>
{
    repo.Create(record);
    return Results.Created($"/btfa/{record.Id}", record);
});

// Note: not part of requirements, also as implemented it will create a new record (due to BTFARecord implementation of Id)
/*app.MapPut("/btfa/{id}", ([FromServices] BTFARepository repo, string id, BTFARecord updatedRecord) =>
{
    var record = repo.GetById(id);
    if (record is null)
    {
        return Results.NotFound();
    }

    repo.Update(updatedRecord);
    return Results.Ok(updatedRecord);
});*/

// Note: not part of requirements
/*app.MapDelete("/btfa/{id}", ([FromServices] BTFARepository repo, string id) =>
{
    repo.Delete(id);
    return Results.Ok();
});*/

app.Run();