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
        private static string jsonUrl = "http://www.batchass.fr/pois.json";
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
            if (!settings.Contains("json.url"))
            {
                settings.Add("json.url", jsonUrl);
            }
            else
            {
                settings["json.url"] = jsonUrl;
            }
        }
        public static void RestoreSettings()
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;
 
            if (!settings.TryGetValue<string>("server.url", out serverUrl))
            {
                serverUrl = "http://www.batchass.fr/h.html";
            }
             if (!settings.TryGetValue<string>("json.url", out jsonUrl))
            {
                jsonUrl = "http://www.batchass.fr/pois.json";
            }
           
        }
        public static string urlServeur()
        {
            return serverUrl;
        }
        public static string urlJson()
        {
            return jsonUrl;
        }
    }
}
