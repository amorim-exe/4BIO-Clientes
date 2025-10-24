using ClienteAPI.Repository;
using ClienteAPI.Service;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api", options =>
        {
            options.HideModels = true;
            options.HideDarkModeToggle = false;
            options.HideModels = true;
            options.Title = "Clientes API";
            options.Layout = ScalarLayout.Classic;
            options.ShowSidebar = true;
        }
    );
}

app.MapGet("/", context =>
{
    context.Response.Redirect("/api", permanent: false);
    return Task.CompletedTask;
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();