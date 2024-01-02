using System;

namespace LvLSystemLC;
public class ItemInfo
{
    public string Name { get; set; } = "";
    public string ItemPath { get; set; } = "";
    public string InfoPath { get; set; } = "";
    public Action<Item> ItemAction { get; set; }
}