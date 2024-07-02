using API.Validation;
using Application;
using Infrastructure;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure();


var app = builder.Build();
var scope = app.Services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

await dbContext.Database.MigrateAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();