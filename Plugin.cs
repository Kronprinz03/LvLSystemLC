using BepInEx;
using System.Security.Permissions;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine;
using System.Reflection;
using System;

namespace LvLSystemLC
{
    
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("evaisa.lethallib")]
    public class Plugin : BaseUnityPlugin
    {
        private static ManualLogSource _logger;
        private static ConfigFile _config;
        
        private void Awake()
        {
            _logger = Logger;
            _config = Config;
            
            
            LvLSystemLC.Config.Load(_config);
            Content.Load(_logger);
            Patches.Patches.Load();
            
            //Loaded Plugin
            _logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        }
       
    }
}
