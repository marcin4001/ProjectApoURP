using UnityEngine;

[CreateAssetMenu(fileName = "AddItemAction", menuName = "Dialogue/AddItemAction")]
public class AddItemAction : ActionDialogue
{
    public SlotItem item;

    public override void Execute()
    {
        Inventory.instance.AddItem(item);
    }
}
