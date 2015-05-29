using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using AzureSqlManager.Models;
using System.Text;

namespace AzureSqlManager.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

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

        // GET: Users
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public JsonResult GetUsers() {
            var result = UserManager.Users;

            List<UserViewModel> model = result.Select(a => new UserViewModel()
            {
                Id = a.Id,
                Email = a.Email
            }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> Register(QuickRegisterViewModel model) {
            bool success = false;
            string msg = "";

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            success = result.Succeeded;
            if (result.Errors.Count() > 0)
            {
                StringBuilder errorSummary = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorSummary.Append(error + "<br>");
                }
                msg = errorSummary.ToString();
            }
            else { 
                msg = "Successfully created";
            }

            return Json(new { message = msg, success = result.Succeeded });
            
        }

        [HttpPost]
        public async Task<JsonResult> Delete(UserViewModel model)
        {
            string msg = "";

            var user = await UserManager.FindByIdAsync(model.Id);
            var result = await UserManager.DeleteAsync(user);

            if (result.Errors.Count() > 0)
            {
                StringBuilder errorSummary = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorSummary.Append(error + "<br>");
                }
                msg = errorSummary.ToString();
            }
            else
            {
                msg = "Successfully deleted.";
            }

            return Json(new { message = msg, success = result.Succeeded });
        }
    }
}