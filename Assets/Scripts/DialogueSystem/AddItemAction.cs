using UnityEngine;

[CreateAssetMenu(fileName = "AddItemAction", menuName = "Dialogue/AddItemAction")]
public class AddItemAction : ActionDialogue
{
    public SlotItem item;

    public override void Execute()
    {
        Inventory.instance.AddItem(item);
        if(item.GetItem() is MiscItem)
        {
            MiscItem miscItem = (MiscItem)item.GetItem();
            if(miscItem.isAmmo)
            {
                HUDController.instance.SetItemSlot();
            }
        }
    }
}
