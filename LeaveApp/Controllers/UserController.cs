using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using LeaveApp.Models;
namespace LeaveApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]User user)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                //check if email exists
                var exists = EmailExists(user.Email);
                if(exists)
                {
                    ModelState.AddModelError("EmailExist", "Email already exists");
                    return View(user);
                }
                //generate activation code
                user.ActivationCode = Guid.NewGuid();

                //Password Hashing
                user.Password = Encrypt.Hash(user.Password);
                user.ConfirmPassword = Encrypt.Hash(user.ConfirmPassword);
                user.IsEmailVerified = false;

                //save to database
                using (LeaveDBEntities db = new LeaveDBEntities())
                {
                    db.Users.Add(user);
                    db.SaveChanges();

                    //send email
                    SendVerificationLink(user.Email, user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link has been sent to your email id " + user.Email;
                    Status = true;
                }
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }

        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (LeaveDBEntities db = new LeaveDBEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                var v = db.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (LeaveDBEntities dc = new LeaveDBEntities())
            {
                var v = dc.Users.Where(a => a.Id == login.Id).FirstOrDefault();
                if (v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }
                    if (string.Compare(Encrypt.Hash(login.Password), v.Password) == 0)
                    {
                        login.RememberMe = false;
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.Id, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Faculty");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [NonAction]
        public bool EmailExists(string email)
        {
            using(LeaveDBEntities db = new LeaveDBEntities())
            {
                var v = db.Users.Where(a => a.Email == email).FirstOrDefault();
                return v == null ? false : true;
            }
        }
        public void SendVerificationLink(string email, string ActivationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + ActivationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("rnsit.leaveapp@gmail.com");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "rnsit123";
            string subject = "Account Activation";

            string body = "<br/><br/>Account has been created. Please click on this link to verify your account<br/><br/>" + link;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}