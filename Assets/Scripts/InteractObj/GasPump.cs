using System.Collections;
using UnityEngine;

public class GasPump : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private Item jerryCanEmptyItem;
    [SerializeField] private Item jerryCanFullItem;
    void Start()
    {

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
        if (!Inventory.instance.PlayerHaveItem(jerryCanEmptyItem.id))
        {
            HUDController.instance.AddConsolelog("You don't have an empty");
            HUDController.instance.AddConsolelog("jerry can");
            return;
        }
        SlotItem jerryCanEmpty = new SlotItem(jerryCanEmptyItem, 1);
        Inventory.instance.RemoveItem(jerryCanEmpty);
        SlotItem jerryCanFull = new SlotItem(jerryCanFullItem, 1);
        Inventory.instance.AddItem(jerryCanFull);
        HUDController.instance.AddConsolelog("You filled the canister");
        HUDController.instance.AddConsolelog("with diesel");
        //StartCoroutine(Refueling());
    }

    private IEnumerator Refueling()
    {
        yield return null;
    }


}
