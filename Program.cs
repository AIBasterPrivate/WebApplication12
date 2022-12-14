using MongoDB.Driver;
using System.Text;
using WebApplication12.Models;
using WebApplication12.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton(s =>
{
    var mongoClient = s.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase(builder.Configuration.GetSection("LBDB")["Name"]);
});
builder.Services.AddSingleton(typeof(IRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

app.MapGet("/", async (HttpContext db) =>
{
    var booksOutOfStorage = app.Services.GetRequiredService<IRepository<BookOutOfStorage>>();
    var booksout = (await booksOutOfStorage.GetAllAsync()).GroupBy(b => new { b.Person.Firstname, b.Person.Middlename, b.Person.Lastname, b.Person.Birthday,b.Person.Address,b.BookReturnDate,b.BookTakeDate,b.Book.Name,b.Book.Author,b.Book.Price});

    db.Response.Headers.ContentType = new Microsoft.Extensions.Primitives.StringValues("text/html; charset=UTF-8");

    var strBuilder = new StringBuilder();
    strBuilder.AppendLine("<img src=\"/images/Logo.png\" alt=\"logo\" width='500' height='300'/>");
    foreach (var book in booksout)
    {
        strBuilder.AppendLine("<div>");
        strBuilder.AppendLine($"<h2>{book.Key.Firstname} {book.Key.Middlename} {book.Key.Lastname} {book.Key.Birthday} {book.Key.Address} {book.Key.BookReturnDate.ToShortDateString()} {book.Key.BookTakeDate.ToShortDateString()} {book.Key.Name} {book.Key.Author} {book.Key.Price}</h2>");
        strBuilder.AppendLine("<ul>");
        strBuilder.AppendLine("</ul>");
        strBuilder.AppendLine("</div>");
    }

    await db.Response.WriteAsync(text: "<html><body>" + strBuilder.ToString() + "<body></html>");
});

app.UseStaticFiles();

app.Run();