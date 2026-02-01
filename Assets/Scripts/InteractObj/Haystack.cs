using UnityEngine;

public class Haystack : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private Item capsuleEmptyItem;
    [SerializeField] private Item capsuleFullItem;

    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public Vector3 GetNearPoint()
    {
        return nearPoint.position;
    }

    public void Use()
    {
        if (!Inventory.instance.PlayerHaveItem(capsuleEmptyItem.id))
        {
            HUDController.instance.AddConsolelog("You don't have an Empty");
            HUDController.instance.AddConsolelog("Seed Capsule");
            return;
        }
        HUDController.instance.AddConsolelog("You filled the capsule");
        HUDController.instance.AddConsolelog("with wheat");
        SlotItem capsuleEmpty = new SlotItem(capsuleEmptyItem, 1);
        Inventory.instance.RemoveItem(capsuleEmpty);
        SlotItem capsuleFull = new SlotItem(capsuleFullItem, 1);
        Inventory.instance.AddItem(capsuleFull);
    }
}
