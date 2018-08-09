using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using ChatProject.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatProject.Web.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ChatProject.Web");
                    UserManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("EmailConfirmation"));
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = token },
                        protocol: Request.Url.Scheme);

                    await UserManager.SendEmailAsync(user.Id, "Confirm email",
                        "Click the link to complete the registration:: <a href=\""
                        + callbackUrl + "\">Complete the registration</a>");

                    return View("DisplayEmail");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ChatProject.Web");
            UserManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("EmailConfirmation"));
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
            
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Wrong password or email");
                }
                else
                {
                    if (user.EmailConfirmed == true)
                    {
                        ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                                DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            //string token = Token(user);
                            return RedirectToAction("ChatPage", "UserChat", user);
                        }
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "email not confirm.");
                    }
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }


        public ActionResult ResetPassword(string userId, string code)
        {
            if (userId == null)
            {
                string currentUserId = User.Identity.GetUserId();
                if (currentUserId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ResetPasswordViewModel model = new ResetPasswordViewModel() { Id = currentUserId };
                return View(model);
            }
            ResetPasswordViewModel resetPasswordViewModelmodel = new ResetPasswordViewModel() { Id = userId };
            return View(resetPasswordViewModelmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var removePassword = UserManager.RemovePassword(model.Id);
            if (removePassword.Succeeded)
            {
                var AddPassword = UserManager.AddPassword(model.Id, model.NewPassword);
                if (AddPassword.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                try
                {
                    var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ChatProject.Web");
                    UserManager.UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("ChangePassword"));
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = token },
                        protocol: Request.Url.Scheme);

                    await UserManager.SendEmailAsync(user.Id, "Forgot password",
                        "Click the link to change your password:: <a href=\""
                        + callbackUrl + "\">Change password</a>");

                    return View("DisplayForgotPassword");
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public JsonResult Token()
        {
            ClaimsIdentity identity = GetIdentity(UserManager.FindById(User.Identity.GetUserId()));
            if (identity == null)
            {
                return Json("invalid");
            }

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(CreateToken(identity));

            var response = new
            {
                access_token = encodedJwt,
            };

            string accessToken = response.access_token;
            return Json(new { token = accessToken }, JsonRequestBehavior.AllowGet);
        }

        public ClaimsIdentity GetIdentity(User user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(
                    claims: claims,
                    authenticationType: "Token",
                    nameType: ClaimsIdentity.DefaultNameClaimType,
                    roleType: ClaimsIdentity.DefaultRoleClaimType
                    );

                return claimsIdentity;
            }
            return null;
        }
        public JwtSecurityToken CreateToken(ClaimsIdentity identity)
        {
            var now = DateTime.Now;
            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            return jwt;
        }

        public class AuthOptions
        {
            public const string ISSUER = "MyAuthServer";
            public const string AUDIENCE = "http://localhost:49971/";
            const string KEY = "scrummakerscrummakerscrummaker";
            public const int LIFETIME = 1800;
            public static SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
            }
        }


        [HttpGet]
        public JsonResult GetUser()
        {
            User currentUser = UserManager.FindById(User.Identity.GetUserId());
            return Json(new { user = currentUser }, JsonRequestBehavior.AllowGet);
        }
    }
}