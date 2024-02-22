
using FluentValidation;
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Managers;
using Grab.A.Seat.BookingAPI.Validator;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Manager;
using Grab.A.Seat.Shared.ManagerConfig;
using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
builder.Services.AddScoped<BaseGetAllManager<GetPaginatedBookingCommand>, GetPaginatedBookingsManager>();
builder.Services.AddScoped<BaseManager<GetBookingByIdCommand>, GetBookingByIdManager>();
builder.Services.AddScoped<BaseManager<UpdateBookingCommand>, UpdateBookingManager>();
builder.Services.AddScoped<BaseManager<DeleteBookingCommand>, DeleteBookingManager>();
builder.Services.AddScoped<BaseGetAllManager<SearchBookingCommand>, SearchBookingsManager>();
builder.Services.AddScoped<BaseManager<SendEmailCommand>, SendBookingConfirmationEmailManager>();



#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//CORS Settings
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder
        //.WithOrigins("http://localhost:8080") // access allowed only from Gateway
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();

        //builder.WithOrigins("https://grabaseatbookingservice.azurewebsites.net/") // access allowed only from Gateway
        //    .AllowAnyHeader()
        //    .AllowAnyMethod();
    });
});

// Add health checks with additional options
builder.Services.AddHealthChecks()
    .AddCheck("custom", () =>
    {
        return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Custom check is healthy!");
    });



var app = builder.Build();

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

// Configure health check endpoint with a response writer
app.Map("/health", context =>
{
    var healthCheckOptions = new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            if (report.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy)
            {

                // Additional checks if the overall status is healthy
                var databaseCheck = await CheckDatabaseAsync(context.RequestServices);
                if (!databaseCheck)
                {
                    // Modify the overall status to Unhealthy
                    context.Response.StatusCode = 503; // Service Unavailable
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Status = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy.ToString(),
                        CustomMessage = "Database connectivity check failed."
                    });
                    return;
                }
            }
                await context.Response.WriteAsJsonAsync(
                new { Status = report.Status.ToString(), CustomMessage = "Health-Check Successful!" });
        }
    };

    app.UseHealthChecks("/health", healthCheckOptions);
});



app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// check database connectivity
async Task<bool> CheckDatabaseAsync(IServiceProvider serviceProvider)
{
    try
    {
        // Resolve the ApplicationDbContext from the service provider
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.CanConnectAsync();
            return true;
        }
    }
    catch (Exception)
    {
        return false;
    }
}