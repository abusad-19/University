using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class UserBLL : IUserBLL
    {
        private readonly UserRepository _repository;
        public UserBLL(UserRepository repository)
        {
            _repository = repository;
        }

        public List<User> GetAll()
        {
            return _repository.GetUsers();
        }
        public string AddUser(User user)
        {
            var temp=_repository.IsEmailUsed(user.UserCode);
            if (temp is true)
                return "undone";

            _repository.AddUser(user);
            _repository.SaveChanges();
            return "done";
        }

        public User? GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }

        public Department? GetDepartmentByDeptCode(int code)
        {
            return _repository.GetDepartmentByDeptCode(code);
        }

        public void DeleteUser(User user)
        {
            _repository.DeleteUser(user);
            _repository.SaveChanges();
        }
        public void UpdateUser(User oldUser, User newUser)
        {
            oldUser.UserCode = newUser.UserCode;
            oldUser.Password= newUser.Password;

            _repository.UpdateUser(oldUser);
            _repository.SaveChanges();
        }

        public Role? GetRoleById(int id)
        {
            return _repository.GetRoleById(id);
        }

        public void AddUserRole(int userId,int roleId)
        {
            UserRole temp = new UserRole();
            temp.UserId = userId;
            temp.RoleId = roleId;
            _repository.AddUserRole(temp);
            _repository.SaveChanges();
        }

        
        public (List<Role>?, List<Role>) GiveRole(int userId)
        {
            var roles = _repository.GetAllRoles();
            var userRoles = _repository.GetASingleUserRoles(userId);
            if (userRoles.Count is 0)
                return (null,roles);

            List<Role> toAdd = new List<Role>();
            List<Role> added = new List<Role>();
            foreach (var item1 in roles)
            {
                bool Isadded = false;
                foreach(var item2 in userRoles)
                {
                    if(item1.Id == item2.RoleId)
                    {
                        Isadded = true;
                        break;
                    }
                }

                if(Isadded is true)
                {
                    added.Add(item1);
                }
                else
                {
                    toAdd.Add(item1);
                }
            }
            return (added, toAdd);
        }

        public void RemoveUserRole(int userId,int roleId)
        {
            var target = _repository.GetUserRoleByUserId_RoleId(userId, roleId);
            if (target != null)
            {
                _repository.RemoveUserRole(target);
                _repository.SaveChanges();
            }
        }

        public (List<Permission>,List<Role>) GetPermissionsAndRolesOfSingleUser(int userId)
        {
            var userRoles = _repository.GetASingleUserRoles(userId);

            List<Permission> userPermissions = new List<Permission>();
            List<int?> permissionsCode= new List<int?>();
            List<Role> roles = new List<Role>();

            foreach (var item in userRoles)
            {
                //rolesId.Add(item.RoleId);
                roles.Add(_repository.GetRoleById(item.RoleId));

                var rolePermissions = _repository.GetRolePermissions(item.RoleId);
                foreach (var permission in rolePermissions)
                {
                    permissionsCode.Add(permission.PermissionId);
                }
            }

            var uniquePermissins=permissionsCode.Distinct().ToList();
            foreach(var item in uniquePermissins)
            {
                var permit = _repository.GetPermission(item);
                if (permit != null)
                {
                    userPermissions.Add(permit);
                }
            }

            return (userPermissions, roles);
        }

        private void CreateRequest(int applicantId, string type, string department)
        {
            var request = new CertificateWithdrawApprovalHistory();
            request.ApplicantId = applicantId;
            request.ApplicantType = type;
            request.Department = department;
            request.RequestCreated=DateTime.Now;
            request.RequestStatus = 1;
            _repository.AddCertificateWithdrawApprovalHistory(request);
            _repository.SaveChanges();
        }

        public void CreateCertificateWithdrawRequest(int applicantId, string type)//here applicantId=studentId,teacherId,employeeId and type=student,teacher,employee
        {
            if(type=="Student")
            {
                var temp = _repository.GetStudentByStudentId(applicantId);
                if(temp is not null)
                {
                    CreateRequest(temp.StudentId, type,temp.Department!);
                }
            }
            else if(type=="Teacher")
            {
                var temp=_repository.GetTeacherByTeacherId(applicantId);
                if(temp is not null)
                {
                    CreateRequest(temp.TeacherId, type,temp.Department!);
                }
            }
            else if( type == "Department_Employee")
            {
                var temp=_repository.GetEmployeeByEmployeeId(applicantId);
                if(temp is not null)
                {
                    CreateRequest(temp.EmployeeId, type, temp.DutyPlace!);
                }
            }
        }

        public List<CertificateWithdrawApprovalHistory> GetRequest(int applicantId, string? deptName)
        {
            if (applicantId > 0)
            {
                return _repository.GetRequestByApplicant(applicantId);
            }
            else if(deptName!=null)
            {
                return _repository.GetRequestByDepartment(deptName);
            }
            else
            {
                return _repository.GetRequest();
            }
        }
        
        public int ApproveOrRejectRequest(int requestId, int approverId, int applicantId, int accept)
        {
            var request=_repository.GetRequestById(requestId);
            if (request is null)
                return 0;

            if (applicantId > 0 && accept==0)
            {
                request.RequestStatus = 0;
                _repository.UpdateCertificateWithdrawApprovalHistory(request);
                _repository.SaveChanges();
                return 1;
            }
            else if (approverId > 0)
            {
                if(accept==1)
                {
                    request.DepartmentalApprove = $"Approved ({DateTime.Now})";
                    _repository.UpdateCertificateWithdrawApprovalHistory(request);
                    _repository.SaveChanges();
                    return 1;
                }
                else
                {
                    request.RequestStatus = 0;
                    request.DepartmentalApprove= $"Rejected ({DateTime.Now})";
                    _repository.UpdateCertificateWithdrawApprovalHistory(request);
                    _repository.SaveChanges();
                    return 1;
                }

            }
            else
            {
                if (accept == 1)
                {
                    request.SyndicateApprove = $"Approved ({DateTime.Now})";
                    _repository.UpdateCertificateWithdrawApprovalHistory(request);
                    _repository.SaveChanges();
                    return 1;
                }
                else
                {
                    request.RequestStatus = 0;
                    request.SyndicateApprove = $"Rejected ({DateTime.Now})";
                    _repository.UpdateCertificateWithdrawApprovalHistory(request);
                    _repository.SaveChanges();
                    return 1;
                }
            }
        }

        public void ReceiveCertificate(int requestId)
        {
            var request=_repository.GetRequestById(requestId);
            if (request is null)
                return;
            
            request.RecievedDate = DateTime.Now;
            request.RequestStatus = 0;
            _repository.UpdateCertificateWithdrawApprovalHistory(request);
            _repository.SaveChanges();
        }
    }
}
