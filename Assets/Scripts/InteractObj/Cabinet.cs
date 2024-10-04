using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour, IUsableObj
{
    [SerializeField] private string isOpenParam = "isOpen";
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private string cabinetName = "cabinet";
    [SerializeField] private int idCabinet = 0;
    [SerializeField] private List<SlotItem> items = new List<SlotItem>();
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        items = ListCabinet.instance.GetListItem(idCabinet);
    }

    public void Use()
    {
        StartCoroutine(Open());
    }

    public IEnumerator Open()
    {
        isOpen = true;
        if (animator != null)
        {
            animator.SetBool(isOpenParam, isOpen);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
        CabinetUI.instance.Show(this);
    }

    public void Close()
    {
        isOpen = false;
        if(animator != null)
            animator.SetBool(isOpenParam, isOpen);
    }

    public Vector3 GetNearPoint()
    {
        if(nearPoint != null)
            return nearPoint.position;
        return transform.position;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public bool CanUse()
    {
        return true;
    }

    public string GetCabinetName()
    {
        return cabinetName;
    }

    public List<SlotItem> GetItems()
    {
        return items;
    }

    public void AddItem(SlotItem item)
    {
        if (item.GetItem() is WeaponItem)
        {
            items.Add(item);
            return;
        }
        bool itemExist = items.Exists(x => x.GetItem().id == item.GetItem().id);
        if(itemExist)
        {
            SlotItem foundItem = items.Find(x => x.GetItem().id == item.GetItem().id);
            int newAmount = foundItem.GetAmount() + item.GetAmount();
            foundItem.SetAmount(newAmount);
            return;
        }
        items.Add(item);
    }

    public void AddNonStackableItem(Item item)
    {
        SlotItem newSlot = new SlotItem(item, 1);
        items.Add(newSlot);
    }

    public void RemoveItem(SlotItem item)
    {
        bool itemExist = items.Exists(x => x.GetItem().id == item.GetItem().id);
        if(itemExist)
        {
            SlotItem foundItem = items.Find(x => x.GetItem().id == item.GetItem().id);
            int newAmount = foundItem.GetAmount() - item.GetAmount();
            if(newAmount > 0)
            {
                foundItem.SetAmount(newAmount);
                return;
            }
            items.Remove(foundItem);
        }
    }

    public void SaveItems()
    {
        ListCabinet.instance.SetListItem(idCabinet, items);
    }
}
