using FirstAPI.Data.DAL;
using FirstAPI.Dtos.CategoryDtos;
using FirstAPI.Dtos.ProductDtos;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation
    (c => c.RegisterValidatorsFromAssemblyContaining<ProductCreateDtoValidator>())
    .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<CategoryCreateDtoValidator>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WebAppDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("default")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
