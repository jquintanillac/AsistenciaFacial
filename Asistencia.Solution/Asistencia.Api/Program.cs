using Asistencia.Api.Extensions;
using Asistencia.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Run Seeder
//using (var scope = app.Services.CreateScope())
//{
//    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
//    await seeder.SeedAsync();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<Asistencia.Api.Middleware.SecurityHeadersMiddleware>();

app.UseAuthentication();
app.UseMiddleware<Asistencia.Api.Middleware.TokenBlacklistMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

