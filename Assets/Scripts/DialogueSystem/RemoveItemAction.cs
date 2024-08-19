using UnityEngine;

[CreateAssetMenu(fileName = "RemoveItemAction", menuName = "Dialogue/RemoveItemAction")]
public class RemoveItemAction : ActionDialogue
{
    public SlotItem item;
    public override void Execute()
    {
        Inventory.instance.RemoveItem(item);
    }
}
