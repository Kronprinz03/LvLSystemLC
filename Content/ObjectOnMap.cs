using System;
using LethalLib.Modules;
using UnityEngine;

namespace LvLSystemLC;

public class ObjectOnMap
{
    
    public string name;
    public string objectPath;
    public Levels.LevelTypes levelFlags;
    public Func<SelectableLevel, AnimationCurve> spawnRateFunction;
    public bool enabled = true;

    public ObjectOnMap(string name, string objectPath, Levels.LevelTypes levelFlags,Func<SelectableLevel, AnimationCurve> spawnRateFunction = null, bool enabled = false)
    {
        this.name = name;
        this.objectPath = objectPath;
        this.levelFlags = levelFlags;
        this.spawnRateFunction = spawnRateFunction;
        this.enabled = enabled;
    }

    public static ObjectOnMap Add(string name, string objectPath, Levels.LevelTypes levelFlags, Func<SelectableLevel, AnimationCurve> spawnRateFunction = null, bool enabled = false)
    {
        ObjectOnMap mapObject = new ObjectOnMap(name, objectPath, levelFlags, spawnRateFunction, enabled);
        return mapObject;
    }


}