using System.Collections;
using UnityEngine;

public class Stove : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private Item rawMeatItem;
    [SerializeField] private Item rawFishItem;
    [SerializeField] private Item porkChop;
    [SerializeField] private Item friedFish;
    [SerializeField] private GameObject pan;
    [SerializeField] private GameObject panFish;
    [SerializeField] private float cookingTime = 2f;
    [SerializeField] private bool withoutbattery = false;
    [SerializeField] private GameObject batteryObj;
    [SerializeField] private int questID;
    private PlayerController player;
    private AudioSource source;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        source = GetComponent<AudioSource>();
        pan.SetActive(false);
        panFish.SetActive(false);
        if (withoutbattery && QuestController.instance.Complete(questID))
        {
            withoutbattery = false;
            return;
        }
        if (withoutbattery && batteryObj != null)
        {           
            batteryObj.SetActive(false);
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
        if (withoutbattery)
        {
            HUDController.instance.AddConsolelog("The stove has no power.");
            return;
        }
        if (!Inventory.instance.PlayerHaveItem(rawMeatItem.id))
        {
            if(Inventory.instance.PlayerHaveItem(rawFishItem.id))
            {
                StartCoroutine(CookingFish());
                return;
            }
            HUDController.instance.AddConsolelog("You don't have any raw");
            HUDController.instance.AddConsolelog("meat or fish.");
            return;
        }
        StartCoroutine(Cooking());
    }

    private IEnumerator Cooking()
    {
        yield return new WaitForEndOfFrame();
        pan.SetActive(true);
        CameraMovement.instance.SetBlock(true);
        player.SetBlock(true);
        SlotItem rawMeatSlot = new SlotItem(rawMeatItem, 1);
        Inventory.instance.RemoveItem(rawMeatSlot);
        if(source != null)
            source.Play();
        yield return new WaitForSeconds(cookingTime);
        pan.SetActive(false);
        SlotItem porkChopSlot = new SlotItem(porkChop, 1);
        Inventory.instance.AddItem(porkChopSlot);
        CameraMovement.instance.SetBlock(false);
        player.SetBlock(false);
        if(source != null)
            source.Stop();
        SteamAchievements.Add("NEW_ACHIEVEMENT_1_2");
    }

    private IEnumerator CookingFish()
    {
        yield return new WaitForEndOfFrame();
        panFish.SetActive(true);
        CameraMovement.instance.SetBlock(true);
        player.SetBlock(true);
        SlotItem fishSlot = new SlotItem(rawFishItem, 1);
        Inventory.instance.RemoveItem(fishSlot);
        if (source != null)
            source.Play();
        yield return new WaitForSeconds(cookingTime);
        panFish.SetActive(false);
        SlotItem friedFishSlot = new SlotItem(friedFish, 1);
        Inventory.instance.AddItem(friedFishSlot);
        CameraMovement.instance.SetBlock(false);
        player.SetBlock(false);
        if (source != null)
            source.Stop();
    }

    public void InsertBattery()
    {
        withoutbattery = false;
        if(batteryObj != null)
            batteryObj.SetActive(true);
    }
}
