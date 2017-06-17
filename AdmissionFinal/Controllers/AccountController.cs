using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AdmissionFinal.Entity;
using AdmissionFinal.Helper;
using AdmissionFinal.Models;

namespace AdmissionFinal.Controllers
{
    public class AccountController : Controller
    {
        AdmissionContext db = new AdmissionContext();

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin,Hall")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model, string role)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    if(role == "Hall")
                    {
                        var hall = new Hall();
                        hall.HallId = Guid.NewGuid();
                        hall.aspnet_Users = db.aspnet_Users.
                            Single(m => m.UserName == model.UserName);
                        hall.UserId = hall.aspnet_Users.UserId;
                        hall.HallName = model.UserName;
                        db.Halls.Add(hall);
                       
                        Roles.AddUserToRole(model.UserName,role);

                        var admissionFee = new AdmissionFee();
                        admissionFee.AdmissionFeeId = Guid.NewGuid();
                        admissionFee.LastDate = DateTime.Now;
                        admissionFee.Hall = hall;
                        admissionFee.HallId = admissionFee.Hall.HallId;
                        admissionFee.Total = 0.0m;
                        db.AdmissionFees.Add(admissionFee);


                        var examFee = new ExamFee();
                        examFee.ExamFeeId = Guid.NewGuid();
                        examFee.LastDate = DateTime.Now;
                        examFee.Hall = hall;
                        examFee.HallId = examFee.Hall.HallId;
                        examFee.Total = 0.0m;
                        db.ExamFees.Add(examFee);

                        db.SaveChanges();
                        new MailSender(model.Email,"User Name:"+model.UserName+"<br/>Password:"+model.Password);

                    }else if(role == "Department")
                    {
                        var department = new Department();
                        department.DepartmentId = Guid.NewGuid();
                        department.aspnet_Users = db.aspnet_Users.
                            Single(m => m.UserName == model.UserName);
                        department.UserId = department.aspnet_Users.UserId;
                        department.DepartmentName = model.UserName;
                        db.Departments.Add(department);
                        db.SaveChanges();

                        Roles.AddUserToRole(model.UserName, role);
                        new MailSender(model.Email, "User Name:" + model.UserName + "<br/>Password:" + model.Password);
                    }else if(role == "Bank")
                    {
                        var bank = new Bank();
                        bank.BankId = Guid.NewGuid();
                        bank.aspnet_Users = db.aspnet_Users.
                            Single(m=>m.UserName==model.UserName);
                        bank.UserId = bank.aspnet_Users.UserId;
                        bank.BankName = model.UserName;
                        db.Banks.Add(bank);
                        db.SaveChanges();

                        Roles.AddUserToRole(model.UserName, role);
                        new MailSender(model.Email, "User Name:" + model.UserName + "<br/>Password:" + model.Password);
                    }else if(role == null && User.IsInRole("Hall"))
                    {
                        //this is a student

                        Roles.AddUserToRole(model.UserName, "Student");
                        var user = db.aspnet_Users.Single(m => m.UserName == model.UserName);
                        UserIdSaver.UserId = user.UserId;
                        UserIdSaver.Password = model.Password;
                        return RedirectToAction("StudentInformation", "Hall");
                    }
                   // FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
