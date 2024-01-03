using System;
using System.Collections.Generic;
using UnityEngine;

namespace LvLSystemLC;

public class AdditionalItem
{
    public string Name { get; set; } = "";
    public string ItemPath { get; set; } = "";
    public string InfoPath { get; set; } = "";
    public Action<Item> ActionOnItem { get; set; } = null;
    
    public bool Enabled { get; set; } = true;
    public AdditionalItem(ItemInfo itemInfo, bool? enabled)
    {
        Name = itemInfo.Name;
        ItemPath = itemInfo.ItemPath;
        InfoPath = itemInfo.InfoPath;
        ActionOnItem = itemInfo.ItemAction;
        Enabled = enabled ?? true;
    }

    public static AdditionalItem Add(ItemInfo itemInfo, bool Enabled)
    {
        
        return new AdditionalItem(itemInfo, Enabled);
    }
}
