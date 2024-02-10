
using FluentValidation;
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Managers;
using Grab.A.Seat.BookingAPI.Validator;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Manager;
using Grab.A.Seat.Shared.ManagerConfig;
using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Utilities
builder.Services.AddLogging();
builder.Services.AddTransient<IValidator<Booking>, BookingValidator>();

#endregion

var serviceProvider = builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


#region Booking
builder.Services.AddScoped<BaseManager<AddBookingCommand>, AddBookingManager>();
builder.Services.AddScoped<BaseGetAllManager<GetAllBookingCommand>, GetAllBookingsManager>();
builder.Services.AddScoped<BaseManager<GetBookingByIdCommand>, GetBookingByIdManager>();
builder.Services.AddScoped<BaseManager<UpdateBookingCommand>, UpdateBookingManager>();
builder.Services.AddScoped<BaseManager<DeleteBookingCommand>, DeleteBookingManager>();

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using var scope = app.Services.CreateScope();
//await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//await dbContext.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API");
        c.RoutePrefix = string.Empty;
    });
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
