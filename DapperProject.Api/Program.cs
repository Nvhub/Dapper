using DapperProject.Api.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddServices(builder.Configuration);
builder.Services.AddDependency();
builder.Services.AddMemoryCache();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Vahid Nikbakht";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
