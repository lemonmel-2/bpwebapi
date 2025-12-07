using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi.Data;
using webapi.Model;
using webapi.Repository;
using webapi.Repository.impl;
using webapi.service;
using webapi.service.impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<GameContext>(options => options.UseSqlite("Data Source=spacesheep.db"));
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IInventoryRepo, InventoryRepo>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy
            .WithOrigins(builder.Configuration["frontend:url"]!)
            .AllowAnyMethod()
            .AllowAnyHeader()
            ;
    });
});



var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GameContext>();
    context.Database.EnsureCreated(); // Ensure database is created
    
    // Check if products already exist to avoid duplicates
    if (!context.Users.Any())
    {
        context.UserCredentials.AddRange(
            new UserCredential("user1", "k8ZgdzoSpfZ4SJGnD7G8AG4nPokHABjSCDt8D9i3IBE=","Za8aZYFdI1d2JvVDTWZIsw==")
        );
        context.Users.AddRange(
            new User("user1")
        );
        context.SaveChanges();
        Console.WriteLine("Sample data added to database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("frontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
