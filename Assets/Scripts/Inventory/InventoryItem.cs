using System;

[Serializable]
public class InventoryItem
{
    public int stackSize;
    public ItemData data;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
