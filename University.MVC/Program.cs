using Microsoft.EntityFrameworkCore;
using University.BLL.Interfaces;
using University.BLL.Services;
using University.DAL.Models;
using University.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

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
builder.Services.AddScoped<IBookBLL,BookBLL>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<IxStudentBLL,xStudentBLL>();
builder.Services.AddScoped<xStudentRepository>();
builder.Services.AddScoped<IUserBLL,UserBLL>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IPermissionBLL, PermissionBLL>();
builder.Services.AddScoped<PermissionRepository>();
builder.Services.AddScoped<IRoleBLL, RoleBLL>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<ILogInBLL, LogInBLL>();
builder.Services.AddScoped<LogInRepository>();
builder.Services.AddScoped<IEmployeeBLL, EmployeeBLL>();
builder.Services.AddScoped<EmployeeRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(options =>
    {
        options.LoginPath = "/LogIn/Index";
        options.AccessDeniedPath= "/LogIn/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    });

// Define Custom Policies for Permissions
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanReadStudentProfile", policy => policy.RequireClaim("Permission", "Read_StudentProfile"));
    options.AddPolicy("CanAccessDepartmentProfile", policy => policy.RequireClaim("Permission", "Read_DepartmentProfile"));
    options.AddPolicy("CanCreateCertificateRequest", policy => policy.RequireClaim("Permission","Create_CertificateRequest"));
    options.AddPolicy("CanRead_ShowRequest", policy => policy.RequireClaim("Permission", "Read_ShowRequest"));
    options.AddPolicy("CanApproveOrRejectRequest", policy => policy.RequireClaim("Permission", "ApproveOrRejectRequest"));
    options.AddPolicy("CanReceiveCertificate", policy => policy.RequireClaim("Permission", "ReceiveCertificate"));
    options.AddPolicy("CanCRUD_ofUser", policy => policy.RequireClaim("Permission", "user_CRUD_operation"));
    options.AddPolicy("CanManagePermissionRole", policy => policy.RequireClaim("Permission", "ManagePermission&Role"));
    options.AddPolicy("CanTeacher_CRUD_operation", policy => policy.RequireClaim("Permission", "teacher_CRUD_operation"));
    options.AddPolicy("CanStudent_CRUD_operation", policy => policy.RequireClaim("Permission", "student_CRUD_operation"));
    options.AddPolicy("CanCourse_CRUD_operation", policy => policy.RequireClaim("Permission", "course_CRUD_operation"));
    options.AddPolicy("CanEmployee_CRUD_operation", policy => policy.RequireClaim("Permission", "employee_CRUD_operation"));
    options.AddPolicy("CanDepartment_CRUD_operation", policy => policy.RequireClaim("Permission", "department_CRUD_operation"));
    options.AddPolicy("CanLibraryManage", policy => policy.RequireClaim("Permission", "LibraryManagement"));

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
