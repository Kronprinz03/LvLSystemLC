using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;
using Unity.Netcode.Components;
using LethalLib.Modules;
using LvLSystemLC.Behaviours;
using System.Linq;
using LvLSystemLC.Extras;


namespace LvLSystemLC
{
    public class Content
    {
        public static AssetBundle MainAssets;
        public static Dictionary<string, GameObject> Prefabs;
        public static GameObject DevMenuPrefab;
        public static List<AdditionalItem> customItemsForShop { get; set; }
        
        
        public static void Load(ManualLogSource logger)
        {
            LoadAssets(logger);
            customItemsForShop = new List<AdditionalItem>()
            {
                // 
                // customItemsForShop.Add("HackingTool", "Assets/Custom/LethalThings/Items/HackingTool/HackingTool.asset", "Assets/Custom/LethalThings/Items/HackingTool/HackingToolInfo.asset", Config.portalGunPrice.Value, enabled: Config.portalGunEnabled.Value)
            };
            foreach (var item in customItemsForShop)
            {
                if (!item.Enabled)
                {
                    continue;
                }

                var itemAsset = MainAssets.LoadAsset<Item>(item.InfoPath);
                if (itemAsset.spawnPrefab.GetComponent<NetworkTransform>() == null &&
                    itemAsset.spawnPrefab.GetComponent<CustomNetworkTransform>() == null)
                {
                    var networkTransform = itemAsset.spawnPrefab.AddComponent<NetworkTransform>();
                    networkTransform.SlerpPosition = false;
                    networkTransform.Interpolate = false;
                    networkTransform.SyncPositionX = false;
                    networkTransform.SyncPositionY = false;
                    networkTransform.SyncPositionZ = false;
                    networkTransform.SyncScaleX = false;
                    networkTransform.SyncScaleY = false;
                    networkTransform.SyncScaleZ = false;
                    networkTransform.UseHalfFloatPrecision = true;
                }

                Prefabs.Add(item.Name, itemAsset.spawnPrefab);
                NetworkPrefabs.RegisterNetworkPrefab(itemAsset.spawnPrefab);
                item.ActionOnItem(itemAsset);
                
                
                var itemInfo = MainAssets.LoadAsset<TerminalNode>(item.InfoPath);
                logger.LogInfo($"Registering shop item {item.Name} with price {((AdditionalItemForShop)item).itemPrice}");
                Items.RegisterShopItem(itemAsset, null, null, itemInfo, ((AdditionalItemForShop)item).itemPrice);
            }
            
            var devMenu = MainAssets.LoadAsset<GameObject>("Assets/Custom/LethalThings/DevMenu.prefab");
            

            NetworkPrefabs.RegisterNetworkPrefab(devMenu);

            DevMenuPrefab = devMenu;

            try
            {
                var types = Assembly.GetExecutingAssembly().GetLoadableTypes();
                foreach (var type in types)
                {
                    var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    foreach (var method in methods)
                    {
                        var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                        if (attributes.Length > 0)
                        {
                            method.Invoke(null, null);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            logger.LogInfo("Custom content loaded!");
        }
        
        private static void LoadAssets(ManualLogSource logger)
        {
            if (MainAssets != null)
            {
                return;
            }
            MainAssets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "lethalthings"));
            logger.LogInfo("Loaded asset bundle");
        }
    }
}
