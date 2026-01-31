using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Generator : MonoBehaviour, IUsableObj
{
    [SerializeField] private Item jerryCan;
    [SerializeField] private MetalDoor metalDoor;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private bool works = false;
    [SerializeField] private int idPickUp;
    [SerializeField] private int idQuest;

    void Start()
    {
        works = PickUpObjList.instance.ExistOnList(idPickUp);
        if(works && metalDoor != null)
        {
            metalDoor.Unlock();
        }
    }

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
        if(works)
        {
            HUDController.instance.AddConsolelog("The generator is working");
            return;
        }
        if (!Inventory.instance.PlayerHaveItem(jerryCan.id))
        {
            HUDController.instance.AddConsolelog("You don't have jerry");
            HUDController.instance.AddConsolelog("can with gasoline.");
            return;
        }

        SlotItem jerryCanSlot = new SlotItem(jerryCan, 1);
        Inventory.instance.RemoveItem(jerryCanSlot);
        PickUpObjList.instance.AddIdToList(idPickUp);
        if(metalDoor != null)
            metalDoor.Unlock();
        works = true;
        QuestController.instance.SetComplete(idQuest);
        HUDController.instance.AddConsolelog("The generator is working");
    }

}
