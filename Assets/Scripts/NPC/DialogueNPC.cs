using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour, IUsableObj
{
    [SerializeField] private DialogueProfile profile;
    [SerializeField] private int indexNode = 0;
    [SerializeField] private Transform nearPoint;
    [SerializeField] private bool haveRifle = false;
    [SerializeField] private string rifleLayer = "Rifle";
    [SerializeField] private bool seller = false;
    [SerializeField] private int idOffer = 0;
    [SerializeField] private List<SlotItem> tradeSlots = new List<SlotItem>();
    private Animator animator;
    private string noSellReply = "I'm sorry. I have nothing for sale.";

    void Start()
    {
        string npcName = profile.npcName;
        NPCObjList.instance.AddNPC(npcName);
        animator = GetComponentInChildren<Animator>();
        if (animator != null && haveRifle)
        {
            int rifleIndex = animator.GetLayerIndex(rifleLayer);
            animator.SetLayerWeight(rifleIndex, 1f);
        }
        if(seller && ListOffers.instance != null)
        {
            tradeSlots = ListOffers.instance.GetListItem(idOffer);
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

    public string GetNPCName()
    {
        return profile.npcName;
    }

    public bool IsSeller()
    {
        return seller;
    }

    public List<SlotItem> GetItems()
    {
        return tradeSlots;
    }

    public void ShowNoSellReply()
    {
        DialogueUI.instance.SetReply(noSellReply);
    }

    public void Use()
    {
        string npcName = profile.npcName;
        indexNode = NPCObjList.instance.GetIndexNode(npcName);
        bool init = NPCObjList.instance.isInit(npcName);
        DialogueController.instance.SetInit(init);
        DialogueController.instance.SetIndexNode(indexNode);
        DialogueController.instance.SetProfile(profile);
        DialogueController.instance.ShowFirstDialogue();
        if(TradeUI.instance != null )
            TradeUI.instance.SetNPC(this);
        NPCObjList.instance.SetInit(npcName);
    }

    public void RemoveItem(SlotItem item)
    {
        bool itemExist = tradeSlots.Exists(x => x.GetItem().id == item.GetItem().id);
        if (itemExist)
        {
            SlotItem foundItem = tradeSlots.Find(x => x.GetItem().id == item.GetItem().id);
            int newAmount = foundItem.GetAmount() - item.GetAmount();
            if (newAmount > 0)
            {
                foundItem.SetAmount(newAmount);
                return;
            }
            tradeSlots.Remove(foundItem);
        }
    }

    public void AddItem(SlotItem item)
    {
        if (item.GetItem() is WeaponItem)
        {
            tradeSlots.Add(item);
            return;
        }
        if (item.GetItem() is ArmorItem)
        {
            tradeSlots.Add(item);
            return;
        }
        bool itemExist = tradeSlots.Exists(x => x.GetItem().id == item.GetItem().id);
        if (itemExist)
        {
            SlotItem foundItem = tradeSlots.Find(x => x.GetItem().id == item.GetItem().id);
            int newAmount = foundItem.GetAmount() + item.GetAmount();
            foundItem.SetAmount(newAmount);
            return;
        }
        tradeSlots.Add(item);
    }

    public void SaveItems()
    {
        if(!seller)
            return;
        ListOffers.instance.SetListItem(idOffer, tradeSlots);
    }

}
