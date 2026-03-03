using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmRestorer : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private GameObject carBattery;
    [SerializeField] private bool inLab;
    [SerializeField] private bool haveCarBattery;
    [SerializeField] private Item carBatteryItem;
    [SerializeField] private Item capsuleItem;
    [SerializeField] private Item farmRestorerItem;
    private PlayerController player;

    private void Start()
    {
        if(GameParam.instance.playerHaveFarmRestorer)
        {
            Destroy(gameObject);
            return;
        }
        haveCarBattery = GameParam.instance.farmRestorerHaveCarBattery;
        if(inLab)
            return;
        player = FindFirstObjectByType<PlayerController>();
        if(carBattery != null && !haveCarBattery)
        {
            carBattery.SetActive(false);
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
        if (inLab)
        {
            HUDController.instance.AddConsolelog("I can’t take the Farm");
            HUDController.instance.AddConsolelog("Restorer. It’s");
            HUDController.instance.AddConsolelog("incomplete.");
            return;
        }
        if(!haveCarBattery)
        {
            if(!Inventory.instance.PlayerHaveItem(carBatteryItem.id))
            {
                HUDController.instance.AddConsolelog("I can’t take the Farm");
                HUDController.instance.AddConsolelog("Restorer. You need");
                HUDController.instance.AddConsolelog("a car battery.");
                return;
            }
            StartCoroutine(Charging());
        }
        else
        {
            if (!Inventory.instance.PlayerHaveItem(capsuleItem.id))
            {
                HUDController.instance.AddConsolelog("I can’t take the Farm");
                HUDController.instance.AddConsolelog("Restorer. You don’t have");
                HUDController.instance.AddConsolelog("a capsule with wheat.");
                return;
            }
            SlotItem slotItem =  new SlotItem(farmRestorerItem, 1);
            Inventory.instance.AddItem(slotItem);
            SlotItem slotItem1 = new SlotItem(capsuleItem, 1);
            Inventory.instance.RemoveItem(slotItem1);
            GameParam.instance.playerHaveFarmRestorer = true;
            Destroy(gameObject);
        }
    }

    private IEnumerator Charging()
    {
        yield return new WaitForEndOfFrame();
        HUDController.instance.AddConsolelog("Charging...");
        player.SetBlock(true);
        SlotItem slotItem = new SlotItem(carBatteryItem, 1);
        Inventory.instance.RemoveItem(slotItem);
        if (carBattery != null)
        {
            carBattery.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        haveCarBattery = true;
        player.SetBlock(false);
        HUDController.instance.AddConsolelog("The Farm Restorer has");
        HUDController.instance.AddConsolelog("been charged");
        GameParam.instance.farmRestorerHaveCarBattery = haveCarBattery;
    }
}
