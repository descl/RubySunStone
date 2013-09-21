using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubySunStoneMobile.Utils
{
    public static class SettingsHelper
    {
        private static string serverUrl = "http://www.batchass.fr/h.html";
        public static void SaveSettings(string urlSrv)
        {
            serverUrl = urlSrv;
            var settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("server.url"))
            {
                settings.Add("server.url", serverUrl);
            }
            else
            {
                settings["server.url"] = serverUrl;
            }
        }
        public static void RestoreSettings()
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;
 
            if (!settings.TryGetValue<string>("server.url", out serverUrl))
            {
                serverUrl = "http://www.batchass.fr/h.html";
            }
            
        }
        public static string urlServeur()
        {
            return serverUrl;
        }
    }
}
