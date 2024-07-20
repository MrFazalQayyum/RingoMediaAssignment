using Microsoft.EntityFrameworkCore;
using RingoMediaAssignment.DAL;
using RingoMediaAssignment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(item=>item.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase")));
builder.Services.AddTransient<EmailService>(provider =>
        new EmailService("smtp.example.com", 587, "your-email@example.com", "your-email-password"));
builder.Services.AddHostedService<ReminderService>();


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
    pattern: "{controller=Departments}/{action=Index}/{id?}");

app.Run();
