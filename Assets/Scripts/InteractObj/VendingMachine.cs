using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class VendingMachine : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private int idCabinet;
    [SerializeField] private List<SlotItem> items = new List<SlotItem>();
    [SerializeField] private SlotItem vendedProduct;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(AfterStart());
    }

    private IEnumerator AfterStart()
    {
        yield return new WaitForSeconds(0.1f);
        items = ListCabinet.instance.GetListItem(idCabinet);
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
        bool itemExist = items.Exists(x => x.GetItem().id == vendedProduct.GetItem().id);
        if (itemExist)
        {
            Inventory.instance.AddItem(vendedProduct);
            HUDController.instance.AddConsolelog("You added 1x VIP Cola");
            if(source != null)
                source.Play();
            SlotItem foundItem = items.Find(x => x.GetItem().id == vendedProduct.GetItem().id);
            int newAmount = foundItem.GetAmount() - vendedProduct.GetAmount();
            if (newAmount > 0)
            {
                foundItem.SetAmount(newAmount);
                return;
            }
            items.Remove(foundItem);
        }
        else
        {
            HUDController.instance.AddConsolelog("The vending machine is");
            HUDController.instance.AddConsolelog("empty");
        }
    }

    public void SaveItems()
    {
        ListCabinet.instance.SetListItem(idCabinet, items);
    }
}
