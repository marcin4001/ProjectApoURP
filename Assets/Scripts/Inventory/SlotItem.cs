using UnityEngine;

[System.Serializable]
public class SlotItem
{
    [SerializeField] private Item item;
    [SerializeField] private int amount;

    public SlotItem(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public Item GetItem()
    {
        return item;
    }

    public int GetAmount() 
    { 
        return amount;
    }

    public void SetItem(Item _item)
    {
        item = _item;
    }

    public void SetAmount(int _amount)
    {
        amount = _amount;
    }
}
