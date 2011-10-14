using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Web.Administration;

namespace IISAdministration
{
    public interface IIisManager
    {
        void CreateSite(Dictionary<string, string> commandLineArguements);
        void CreateAppPool(Dictionary<string, string> commandLineArguements);
    }

    public class IisManager : IIisManager
    {
        private readonly ServerManager iisManager;
        public IisManager()
        {
            iisManager = new ServerManager();
        }

        public void CreateSite(Dictionary<string, string> commandLineArguements)
        {
            var sites = iisManager.Sites;
            foreach (var site in sites.Where(site => site.Name == commandLineArguements["siteName"]))
            {
                iisManager.Sites.Remove(site);
                break;
            }

            iisManager.Sites.Add(commandLineArguements["siteName"], "http", string.Format("*:80:{0}", commandLineArguements["siteName"]), commandLineArguements["path"]);
            iisManager.Sites[commandLineArguements["siteName"]].ApplicationDefaults.ApplicationPoolName = commandLineArguements["appPoolName"];
            iisManager.Sites[commandLineArguements["siteName"]].ServerAutoStart = true;
            
            iisManager.CommitChanges();
        }

        public void CreateAppPool(Dictionary<string, string> commandLineArguements)
        {
            if (string.IsNullOrWhiteSpace(commandLineArguements["appPoolName"]))
                throw new ArgumentNullException("AppPoolName", "You need to pass a site name and an appPool at the very least");

            var appPools = iisManager.ApplicationPools;
            foreach (var appPool in appPools.Where(appPool => appPool.Name == commandLineArguements["appPoolName"]))
            {
                iisManager.ApplicationPools.Remove(appPool);
                break;
            }

            iisManager.ApplicationPools.Add(commandLineArguements["appPoolName"]);

            iisManager.ApplicationPools[commandLineArguements["appPoolName"]].Enable32BitAppOnWin64 = Get32BitStatus(commandLineArguements);
            iisManager.ApplicationPools[commandLineArguements["appPoolName"]].ManagedRuntimeVersion = GetRunTimeVersion(commandLineArguements);

            iisManager.ApplicationPools[commandLineArguements["appPoolName"]].AutoStart = true;
            iisManager.CommitChanges();
        }

        private static string GetRunTimeVersion(Dictionary<string, string> commandLineArguements)
        {
            var runTimeVersion = commandLineArguements["frameworkVersion"];
            return runTimeVersion;
        }

        private static bool Get32BitStatus(Dictionary<string, string> commandLineArguements)
        {
            var get32BitStatus = false;
            if (Boolean.TryParse(commandLineArguements["enable32Bit"], out get32BitStatus)) ;

            return get32BitStatus;
        }

    }
}
