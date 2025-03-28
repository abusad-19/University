﻿using Microsoft.EntityFrameworkCore;
namespace University.DAL.Models
{
    public class appDBcontext : DbContext
    {
        public appDBcontext(DbContextOptions<appDBcontext>options) 
            :base(options) 
        { 
        }

        public DbSet<Student> StudentTable { get; set; }
        public DbSet<Teacher> TeacherTable { get; set;}
        public DbSet<Department> DepartmentTable { get; set; }
        public DbSet<Course> CourseTable { get; set;}
        public DbSet<StudentResult> StudentResultTable { get; set;}
        public DbSet<StudentResultForYear> StudentResultForYearTable { get;set; }
        public DbSet<Book> BookTable { get; set; }
        public DbSet<Cart> CartTable { get; set; }
        public DbSet<LendBook> LendBookTable { get; set;}
        public DbSet<User> UserTable { get; set; }
        public DbSet<Permission> PermissionTable { get; set; }
        public DbSet<Role> RoleTable { get; set; }
        public DbSet<RolePermissions> RolePermissionsTable { get; set; }
        public DbSet<UserRole> UserRoleTable { get; set; }
        public DbSet<Employee> EmployeeTable { get; set; }
        public DbSet<CertificateWithdrawApprovalHistory> CertificateWithdrawApprovalHistoryTable { get; set; }
    }
}
