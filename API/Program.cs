using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

//app.UseHttpsRedirection(); Don't need this now, from http to https

//app.UseAuthorization(); Is not doing anything here

// NEXT LINE: Lesson 76.  Setting app to use the ExceptionMiddleware
app.UseMiddleware<ExceptionMiddleware>();

/* NEXT LINE: Lesson 24. */
app.UseCors(corsBuilder => corsBuilder.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("https://localhost:4200"));

/* NEXT TWO LINES: Added in lesson 44 
    They must be after app.UseCors() and before app.MapControllers
*/

app.UseAuthentication(); // Do you have a token?
app.UseAuthorization();  // You have a token, let see what you can do.


app.MapControllers();

// NEXT LINE: Lesson 88
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try {
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
} catch (Exception ex) {
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error ocurred during migration");
}

// Lesson 88 finishes here.

app.Run();
