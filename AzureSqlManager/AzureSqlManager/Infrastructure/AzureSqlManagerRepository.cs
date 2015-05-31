using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.Sql.Models;
using Microsoft.WindowsAzure.Management.Sql;

namespace AzureSqlManager.Infrastructure
{
    public class AzureSqlManagerRepository
    {
        //to download the cert https://manage.windowsazure.com/publishsettings/

        private SubscriptionCloudCredentials getCredentials(string subscriptionId, string path)
        {

            var cert = new X509Certificate2(path, "CERTIFICATE_PASSWORD_HERE", X509KeyStorageFlags.MachineKeySet);
            return new CertificateCloudCredentials(subscriptionId, cert);
        }

        public async Task<ServerListResponse> GetServers(string subscriptionId, string path)
        {
            SqlManagementClient client = new SqlManagementClient(getCredentials(subscriptionId, path));
            var servers = await client.Servers.ListAsync();

            client.Dispose();
            return servers;
        }

        public async Task<FirewallRuleListResponse> GetFirewallRules(string subscriptionId, string serverName, string path)
        {
            SqlManagementClient client = new SqlManagementClient(getCredentials(subscriptionId, path));
            var rules = await client.FirewallRules.ListAsync(serverName);

            client.Dispose();
            return rules;
        }

        public async Task<FirewallRuleCreateResponse> AddFireWallRule(string subscriptionId, string serverName, string ip, string user, string path)
        {

            SqlManagementClient client = new SqlManagementClient(getCredentials(subscriptionId, path));

            var rule = new FirewallRuleCreateParameters()
            {
                Name = string.Format("{0}-{1}", user, DateTime.UtcNow.ToShortDateString().Replace("/", "-")),
                StartIPAddress = ip,
                EndIPAddress = ip
            };

            var result = await client.FirewallRules.CreateAsync(serverName, rule);

            client.Dispose();

            return result;
        }

        public async Task<AzureOperationResponse> DeleteFirewallRule(string subscriptionId, string serverName, string ruleName, string path)
        {
            SqlManagementClient client = new SqlManagementClient(getCredentials(subscriptionId, path));

            var result = await client.FirewallRules.DeleteAsync(serverName, ruleName);

            client.Dispose();

            return result;
        }
    }
}