using System;

namespace LvLSystemLC;

public class AdditionalItemForShop : AdditionalItem
{
    public int itemPrice = 0;
    public AdditionalItemForShop(ItemInfo itemInfo) : base(itemInfo)
    {
        this.itemPrice = itemPrice;
    }
    

    public AdditionalItemForShop(string name, string itemPath, string infoPath = null, int itemPrice = 0,
        Action<Item> action = null) : base(new ItemInfo(name, itemPath, infoPath, action))
    {
        this.itemPrice = itemPrice;
    }

    public static AdditionalItemForShop Add(string name, string itemPath, string infoPath = null, int itemPrice = 0,
        Action<Item> action = null, bool enabled = true)
    {
        AdditionalItemForShop item = new AdditionalItemForShop(name, itemPath, infoPath, itemPrice, action);
        item.Enabled = enabled;
        return item;
    }
}

