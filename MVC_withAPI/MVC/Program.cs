using Npgsql;
using repositories.Implementations;
using repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserInterface, UserRepositories>();
builder.Services.AddScoped<IContactInterface, ContactRepository>();
builder.Services.AddScoped<NpgsqlConnection>((UserRepositories) =>
{
    var connectionString = UserRepositories.GetRequiredService<IConfiguration>().GetConnectionString("pgconn");
    return new NpgsqlConnection(connectionString);
});
builder.Services.AddScoped<NpgsqlConnection>((ContactRepository) =>
{
    var connectionString = ContactRepository.GetRequiredService<IConfiguration>().GetConnectionString("pgconn");
    return new NpgsqlConnection(connectionString);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
