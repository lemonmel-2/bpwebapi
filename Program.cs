using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<GameContext>(options => options.UseSqlite("Data Source=spacesheep.db"));

var app = builder.Build();

// Seed the database
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<GameContext>();
//     context.Database.EnsureCreated(); // Ensure database is created
    
//     // // Check if products already exist to avoid duplicates
//     // if (!context.Users.Any())
//     // {
//     //     context.Users.AddRange(
//     //         new User("user1", "testpwd","tstsalt")
//     //     );
//     //     context.SaveChanges();
//     //     Console.WriteLine("Sample data added to database.");
//     // }
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
