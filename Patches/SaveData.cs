using LvLSystemLC.Behaviours;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using LvLSystemLC.Behaviours;
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace LethalLib.Modules
{
    public class SaveData
    {
        public static void Init()
        {
            On.GameNetworkManager.ResetSavedGameValues += GameNetworkManager_ResetSavedGameValues;
            On.GameNetworkManager.SaveItemsInShip += GameNetworkManager_SaveItemsInShip;
            On.StartOfRound.LoadShipGrabbableItems += StartOfRound_LoadShipGrabbableItems;
        }

        public static List<string> saveKeys = new List<string>();

        private static void StartOfRound_LoadShipGrabbableItems(On.StartOfRound.orig_LoadShipGrabbableItems orig, StartOfRound self)
        {
            orig(self);

            SaveableObject[] saveableObjects = UnityEngine.Object.FindObjectsOfType<SaveableObject>();
            SaveableNetworkBehaviour[] saveableNetworkBehaviours = UnityEngine.Object.FindObjectsOfType<SaveableNetworkBehaviour>();

            foreach (var item in saveableObjects)
            {
                item.LoadObjectData();
            }

            foreach (var item in saveableNetworkBehaviours)
            {
                item.LoadObjectData();
            }

            if (ES3.KeyExists("LethalLibItemSaveKeys", GameNetworkManager.Instance.currentSaveFileName))
            {
                saveKeys = ES3.Load<List<string>>("LethalLibItemSaveKeys", GameNetworkManager.Instance.currentSaveFileName);
            }

        }

        private static void GameNetworkManager_SaveItemsInShip(On.GameNetworkManager.orig_SaveItemsInShip orig, GameNetworkManager self)
        {
            orig(self);

            SaveableObject[] saveableObjects = UnityEngine.Object.FindObjectsOfType<SaveableObject>();
            SaveableNetworkBehaviour[] saveableNetworkBehaviours = UnityEngine.Object.FindObjectsOfType<SaveableNetworkBehaviour>();

            foreach (var item in saveableObjects)
            {
                item.SaveObjectData();
            }

            foreach (var item in saveableNetworkBehaviours)
            {
                item.SaveObjectData();
            }

            ES3.Save<List<string>>("LethalLibItemSaveKeys", saveKeys, GameNetworkManager.Instance.currentSaveFileName);

        }

        private static void GameNetworkManager_ResetSavedGameValues(On.GameNetworkManager.orig_ResetSavedGameValues orig, GameNetworkManager self)
        {
            orig(self);

            // delete save keys
            foreach (var key in saveKeys)
            {
                ES3.DeleteKey(key, GameNetworkManager.Instance.currentSaveFileName);
            }

            saveKeys.Clear();
        }

        public static void SaveObjectData<T>(string key, T data, int objectId)
        {
            List<T> values = new List<T>();

            var lethalthingssave = "LethalThingsSave_";
            if (ES3.KeyExists(lethalthingssave + key, GameNetworkManager.Instance.currentSaveFileName))
            {
                values = ES3.Load<List<T>>(lethalthingssave + key, GameNetworkManager.Instance.currentSaveFileName);
            }

            List<int> objectIds = new List<int>();
            if (ES3.KeyExists("LethalThingsSave_objectIds_" + key, GameNetworkManager.Instance.currentSaveFileName))
            {
                objectIds = ES3.Load<List<int>>("LethalThingsSave_objectIds_" + key, GameNetworkManager.Instance.currentSaveFileName);
            }

            values.Add(data);
            objectIds.Add(objectId);

            if (!saveKeys.Contains(lethalthingssave + key))
            {
                saveKeys.Add(lethalthingssave + key);
            }

            if (!saveKeys.Contains("LethalThingsSave_objectIds_" + key))
            {
                saveKeys.Add("LethalThingsSave_objectIds_" + key);
            }



            ES3.Save<List<T>>(lethalthingssave + key, values, GameNetworkManager.Instance.currentSaveFileName);
            ES3.Save<List<int>>("LethalThingsSave_objectIds_" + key, objectIds, GameNetworkManager.Instance.currentSaveFileName);
        }

        public static T LoadObjectData<T>(string key, int objectId)
        {
            List<T> values = new List<T>();

            if (ES3.KeyExists("LethalThingsSave_" + key, GameNetworkManager.Instance.currentSaveFileName))
            {
                values = ES3.Load<List<T>>("LethalThingsSave_" + key, GameNetworkManager.Instance.currentSaveFileName);
            }

            List<int> objectIds = new List<int>();
            if (ES3.KeyExists("LethalThingsSave_objectIds_" + key, GameNetworkManager.Instance.currentSaveFileName))
            {
                objectIds = ES3.Load<List<int>>("LethalThingsSave_objectIds_" + key, GameNetworkManager.Instance.currentSaveFileName);
            }

            if (!saveKeys.Contains("LethalThingsSave_" + key))
            {
                saveKeys.Add("LethalThingsSave_" + key);
            }

            if (!saveKeys.Contains("LethalThingsSave_objectIds_" + key))
            {
                saveKeys.Add("LethalThingsSave_objectIds_" + key);
            }

            // check if index exists
            if (objectIds.Contains(objectId))
            {
                int index = objectIds.IndexOf(objectId);
                return values[index];
            }
            else
            {
                return default(T);
            }
        }


    }
}