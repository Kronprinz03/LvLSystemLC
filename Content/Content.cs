using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;
using Unity.Netcode.Components;
using LethalLib.Modules;
using LvLSystemLC.Behaviours;
using LethalLib.Extras;
using LvLSystemLC.Extras;


namespace LvLSystemLC
{
    public class Content
    { 
        public static List<AdditionalItem> customItems { get; set; }
        
        public static void Load(ManualLogSource logger)
        {
            Dictionary<string, GameObject> Prefabs = null;
            GameObject DevMenuPrefab = null;
            AssetBundle MainAssets = null;
            
            LoadAssets(logger, MainAssets);
            LoadAdditionalItems(logger, Prefabs, DevMenuPrefab, MainAssets);
            LoadAdditionalMapObject(logger, Prefabs, DevMenuPrefab, MainAssets);
            
            var devMenu = MainAssets.LoadAsset<GameObject>("Assets/Custom/Hello/DevMenu.prefab");
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

        private static void LoadAdditionalMapObject(ManualLogSource logger, Dictionary<string, GameObject> Prefabs, GameObject DevMenuPrefab, AssetBundle MainAssets)
        {
            var objectOnMap = new List<ObjectOnMap>()
            {
                ObjectOnMap.Add("Portal", "" +
                                          "Assets/Custom/LethalThings/hazards/TeleporterTrap/TeleporterTrap.asset" +
                                          "", Levels.LevelTypes.All, (level) => { 
                    return new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 4));
                }, Config.portalGunEnabled.Value)
            };
            foreach (var mapObject in objectOnMap)
            {
                if (!mapObject.enabled)
                {
                    continue;
                }
                var mapObjectAsset = MainAssets.LoadAsset<SpawnableMapObjectDef>(mapObject.objectPath);
                NetworkPrefabs.RegisterNetworkPrefab(mapObjectAsset.spawnableMapObject.prefabToSpawn);

                Prefabs.Add(mapObject.name, mapObjectAsset.spawnableMapObject.prefabToSpawn);

                MapObjects.RegisterMapObject(mapObjectAsset, mapObject.levelFlags, mapObject.spawnRateFunction);
            }
        }
        private static void LoadAdditionalItems(ManualLogSource logger, Dictionary<string, GameObject> Prefabs, GameObject DevMenuPrefab, AssetBundle MainAssets)
        {
            customItems = new List<AdditionalItem>()
            {
                AdditionalItemForShop.Add("PortalGun", "Assets/Custom/LethalThings/Items/HackingTool/HackingTool.asset",
                    "Assets/Custom/LethalThings/Items/HackingTool/HackingToolInfo.asset", Config.portalGunPrice.Value,
                    action: (item) =>
                    {
                        NetworkPrefabs.RegisterNetworkPrefab(
                            item.spawnPrefab.GetComponent<PortelGun>().portalOrb);
                    },Config.portalGunEnabled.Value)
            };   
            
            foreach (var item in customItems)
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
                
                
                if(item is AdditionalItemForShop)
                {
                    var itemInfo = MainAssets.LoadAsset<TerminalNode>(item.InfoPath);
                    logger.LogInfo($"Registering shop item {item.Name} with price {((AdditionalItemForShop)item).itemPrice}");
                    Items.RegisterShopItem(itemAsset, null, null, itemInfo, ((AdditionalItemForShop)item).itemPrice);
                }
            }
        } 
        private static void LoadAssets(ManualLogSource logger, AssetBundle mainAssets)
        {
            if (mainAssets != null)
            {
                return;
            }
            mainAssets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "lethalthings"));
            logger.LogInfo("Loaded asset bundle");
        }
    }
}
