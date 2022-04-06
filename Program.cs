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

class BTFARecord
{
    private static long Records = 0;

    private Double _Income;
    public Double Income
    {
        get
        {
            return _Income;
        }
        set
        {
            _Income = value;
            Btfa = calculateBtfa(_Income);
        }
    }
    public string Id { get; private set; }

    public Double Btfa { get; private set; }


    public BTFARecord()
    {
        System.Threading.Interlocked.Increment(ref Records);
        Id = "BESK2022_" + Records;
    }

    private Double calculateBtfa(Double income)
    {
        Double Result;
        const Double PBB = 48_300;

        // Round down to nearest 100
        income = Math.Round(income / 100, MidpointRounding.ToNegativeInfinity) * 100;

        switch (income)
        {
            case <= 0.99 * PBB:
                Result = 0.423 * PBB;
                break;
            case <= 2.72 * PBB:
                Result = 0.423 * PBB + 0.2 * (income - 0.99 * PBB);
                break;
            case <= 3.11 * PBB:
                Result = 0.77 * PBB;
                break;
            case <= 7.88 * PBB:
                Result = 0.77 * PBB - 0.1 * (income - 3.11 * PBB);
                break;
            case > 7.88 * PBB:
                Result = 0.293 * PBB;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Result = Math.Round(Result / 100, MidpointRounding.ToPositiveInfinity) * 100;
        Result = Math.Min(income, Result);

        return Result;
    }
}

class BTFARepository
{
    private readonly Dictionary<string, BTFARecord> _records = new();

    public void Create(BTFARecord record)
    {
        if (record == null)
        {
            return;
        }

        _records[record.Id] = record;
    }

    public BTFARecord GetById(string id)
    {
        if (_records.ContainsKey(id))
            return _records[id];
        return null;
    }

    public List<BTFARecord> GetAll()
    {
        return _records.Values.ToList();
    }

    public void Update(BTFARecord record)
    {
        var existingRecord = GetById(record.Id);
        if (existingRecord == null)
        {
            return;
        }

        _records[record.Id] = record;
    }

    public void Delete(string id)
    {
        _records.Remove(id);
    }
}