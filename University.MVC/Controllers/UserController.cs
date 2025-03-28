﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.BLL.Interfaces;
using University.DAL.Models;
using University.MVC.View_Models;

namespace University.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserBLL _userBll;
        public UserController(IUserBLL userBll)
        {
            _userBll = userBll;
        }

        [Authorize(Policy = "CanCRUD_ofUser")]
        public IActionResult Index()
        {
            return View(_userBll.GetAll());
        }
        
        [Authorize(Policy = "CanCRUD_ofUser")]
        public IActionResult Create(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        [Authorize(Policy = "CanCRUD_ofUser")]
        [HttpPost]
        public IActionResult Create(User user)
        {
            if(user.UserCode <=0 || user.Password==null)
                return RedirectToAction(nameof(Create),
                    new { errorMessage ="Please enter UserCode & Password"});

            var status=_userBll.AddUser(user);
            if(status== "undone")
                return RedirectToAction(nameof(Create),
                    new { errorMessage = "This UserCode is already used. Please use another email" });

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanCRUD_ofUser")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return NotFound();
            var target=_userBll.GetUserById(id);
            if (target is null)
                return NotFound();
            return View(target);
        }

        [Authorize(Policy = "CanCRUD_ofUser")]
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            if(id<=0) return NotFound();

            var target = _userBll.GetUserById(id);
            if (target is null)
                return NotFound();
            
            _userBll.DeleteUser(target);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanCRUD_ofUser")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
                return NotFound();
            var target =_userBll.GetUserById(id);
            if(target is null)
                return NotFound();
            return View(target);
        }

        [Authorize(Policy = "CanCRUD_ofUser")]
        [HttpPost, ActionName("Edit")]
        public IActionResult Update(int id, [Bind("UserCode,Password,UserType")] User user)
        {
            if(id<=0 || user is null)
                return NotFound();

            var target = _userBll.GetUserById(id);
            if (target is null)
                return NotFound();

            _userBll.UpdateUser(target,user);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult GiveRole(int id)//user id
        {
            if(id<=0)
                return NotFound();
            var target = _userBll.GetUserById(id);
            if (target is null)
                return NotFound();

            ViewBag.userId=id;
            var temp=_userBll.GiveRole(id);
            if(temp.Item1  is null)
                return View(temp.Item2);

            ViewBag.added = temp.Item1;
            return View(temp.Item2);
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult GivePermit(int userId,int roleId)
        {
            if(userId<=0 || roleId<=0)
                return NotFound();
            var user= _userBll.GetUserById(userId);
            var permission=_userBll.GetRoleById(roleId);
            if(user is null || permission is null)
                return NotFound();

            _userBll.AddUserRole(userId,roleId);
            return RedirectToAction(nameof(GiveRole), new {id=userId});
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult RemovePermit(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
                return NotFound();
            _userBll.RemoveUserRole(userId,roleId);
            return RedirectToAction(nameof(GiveRole), new {id=userId});
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult ShowRolesAndPermissions(int id)//userId
        {
            if(id<=0)
                return NotFound();

            var user=_userBll.GetUserById(id);
            if (user is null)
                return NotFound();
            var temp=_userBll.GetPermissionsAndRolesOfSingleUser(id);
            ViewBag.Permissions=temp.Item1;
            ViewBag.Roles=temp.Item2;
            return View();
        }

        [Authorize(Policy = "CanCreateCertificateRequest")]
        //this method canbe used by student,teacher & employee
        public IActionResult CreateCertificateWithdrawRequest(int applicantId, string type)//here applicantId=studentId,teacherId,employeeId and type=student,teacher,employee
        {
            if(applicantId <= 0)
                return NotFound();
          
            _userBll.CreateCertificateWithdrawRequest(applicantId, type);
            return RedirectToAction(nameof(ShowRequest), new { applicantId = applicantId });
        }

        [Authorize(Policy = "CanRead_ShowRequest")]
        public IActionResult ShowRequest(int applicantId,int deptCode)
        {
            var dept = _userBll.GetDepartmentByDeptCode(deptCode);
            List<CertificateWithdrawApprovalHistory> requestList;
            var Model = new ShowRequest_ViewModel();

            if (applicantId>0)
            {
                requestList = _userBll.GetRequest(applicantId, null);
                Model.RequestList = requestList;
                Model.ApplicantId = applicantId;
                Model.deptCode = 0;
            }  
            else if(dept!=null)
            {
                requestList = _userBll.GetRequest(0, dept.DepartmentName);
                Model.RequestList = requestList;
                Model.ApplicantId = 0;
                Model.deptCode = deptCode;
            }
            else
            {
                requestList = _userBll.GetRequest(0,null);
                Model.RequestList = requestList;
                Model.ApplicantId = 0;
                Model.deptCode = 0;
            }

            return View(Model);
        }

        [Authorize(Policy = "CanApproveOrRejectRequest")]
        public IActionResult ApproveOrRejectRequest(int requestId, int approverId, int applicantId, int accept)
        {
            if(requestId <= 0 || accept < 0 || accept > 1)
                return NotFound();

            var operation=_userBll.ApproveOrRejectRequest(requestId,approverId, applicantId, accept);
            
            if (operation == 0)
                return NotFound();
            else if(applicantId> 0)
            {
                return RedirectToAction(nameof(ShowRequest), new {applicantId= applicantId});
            }
            else if(approverId>0)
            {
                return RedirectToAction(nameof(ShowRequest), new { deptCode = approverId });
            }
            else
            {
                return RedirectToAction(nameof(ShowRequest));
            }
        }

        [Authorize(Policy = "CanReceiveCertificate")]
        public IActionResult ReceiveCertificate(int requestId, int applicantId)//here applicantId=studentId,teacherId,employeeId and type=student,teacher,employee
        {
            if (requestId <= 0 || applicantId<=0)
                return NotFound();

            _userBll.ReceiveCertificate(requestId);
            return RedirectToAction(nameof(ShowRequest), new { applicantId = applicantId });
        }
    }
}
