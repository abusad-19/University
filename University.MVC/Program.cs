using Microsoft.EntityFrameworkCore;
using University.BLL.Services;
using University.DAL.Models;
using University.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<appDBcontext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("appDBcontext") ?? throw new InvalidOperationException("connection string 'appDBcontext' not found.")));

builder.Services.AddScoped<StudentBLL>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<CourseBLL>();
builder.Services.AddScoped<DepartmentBLL>();
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<TeacherBLL>();
builder.Services.AddScoped<TeacherRepository>();
builder.Services.AddScoped<xDepartmentBLL>();
builder.Services.AddScoped<xDepartmentRepository>();
builder.Services.AddScoped<BookBLL>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<xStudentBLL>();
builder.Services.AddScoped<xStudentRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
