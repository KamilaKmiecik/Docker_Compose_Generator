using Docker_Compose_Generator.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddScoped<IDockerComposeService, DockerComposeService>();
builder.Services.AddServices();


// Dodaj kontrolery i widoki
builder.Services.AddControllersWithViews();

//
//builder.Services.AddDbContext<DockerComposeContext>(options =>
//   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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







