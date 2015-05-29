using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureSqlManager.Infrastructure;
using System.Threading.Tasks;

namespace AzureSqlManager.Controllers
{
    public class ServersController : Controller
    {
        /// <summary>
        /// Path of your certificate, I usually put the certificate in App_Data Folder
        /// </summary>
        /// <returns></returns>
        private string BuildPath()
        {
            return Server.MapPath("~/App_Data/HamakLabsMgmtCert.pfx");
        }

        /// <summary>
        /// azure account subscription ID
        /// </summary>
        private static string _subscriptionId = "f81d2831-f836-4d57-afee-d00a366cce1f";

        // GET: Servers
        public async Task<ActionResult> Index()
        {
            AzureSqlManagerRepository azureSqlManagerRepository = new AzureSqlManagerRepository();
            var model = await azureSqlManagerRepository.GetServers(_subscriptionId, BuildPath());

            return View(model);
        }

        public async Task<ActionResult> FirewallRules(string server) {
            AzureSqlManagerRepository azureSqlManagerRepository = new AzureSqlManagerRepository();
            var model = await azureSqlManagerRepository.GetFirewallRules(_subscriptionId, server, BuildPath());

            ViewBag.IP = Request.UserHostAddress;
            ViewBag.ServerName = server;

            return View(model);
        }

        public async Task<ActionResult> SQLAddFirewallRule(string server)
        {
            
            string user = string.Format("User-{0}",User.Identity.Name);

            AzureSqlManagerRepository azureSqlManagerRepository = new AzureSqlManagerRepository();
            var rules = await azureSqlManagerRepository.GetFirewallRules(_subscriptionId, server, BuildPath());

            foreach (var r in rules)
            {
                if (r.Name.StartsWith(user))
                {
                   await azureSqlManagerRepository.DeleteFirewallRule(_subscriptionId, server, r.Name, BuildPath());
                }
            }

            string ip = Request.UserHostAddress;
            string IP = Request.UserHostName;

            var result = await azureSqlManagerRepository.AddFireWallRule(_subscriptionId, server, ip, user, BuildPath());

            return RedirectToAction("FirewallRules", new { serverName = server });
        }
    }
}