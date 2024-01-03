using BepInEx.Configuration;

namespace LvLSystemLC;

public class Config
{
    public static ConfigEntry<bool> portalGunEnabled;
    public static ConfigEntry<int> portalGunWeight;
    public static ConfigEntry<int> portalGunPrice;

    public static void Load(ConfigFile configFile)
    {
        portalGunPrice = configFile.Bind<int>("Item", "PortalGunPrice", 1000, "Where do you think you are going?");
        portalGunWeight = configFile.Bind<int>("Item", "PortalGunWeight", 10, "Where do you think you are going?");
        portalGunEnabled = configFile.Bind<bool>("Item", "PortalGunEnable", true, "Where do you think you are going?"); 
    }
}