using Microsoft.EntityFrameworkCore;
using University.BLL.Interfaces;
using University.BLL.Services;
using University.DAL.Models;
using University.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<appDBcontext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("appDBcontext") ?? throw new InvalidOperationException("connection string 'appDBcontext' not found.")));

builder.Services.AddScoped<IStudentBLL,StudentBLL>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<ICourseBLL, CourseBLL>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<IDepartmentBLL,DepartmentBLL>();
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<ITeacherBLL,TeacherBLL>();
builder.Services.AddScoped<TeacherRepository>();
builder.Services.AddScoped<IxDepartmentBLL,xDepartmentBLL >();
builder.Services.AddScoped<xDepartmentRepository>();
builder.Services.AddScoped<IBookBLL, BookBLL>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<IxStudentBLL,xStudentBLL>();
builder.Services.AddScoped<xStudentRepository>();
builder.Services.AddScoped<IUserBLL,UserBLL>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IPermissionBLL, PermissionBLL>();
builder.Services.AddScoped<PermissionRepository>();

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
