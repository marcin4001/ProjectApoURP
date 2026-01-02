using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Device : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearpoint;
    [SerializeField] private GameObject mainObj;
    [SerializeField] private int keyID;
    [SerializeField] private SlotItem part;
    [SerializeField] private GameObject partObj;
    [SerializeField] private int idPickUp;
    [SerializeField] private DevicesPart devicesPart = DevicesPart.carBattery;
    [SerializeField] private bool forRepair = false;
    [SerializeField] private int repairPartID;
    [SerializeField] private List<ActionDialogue> actions = new List<ActionDialogue>();
    [SerializeField] private bool mustHaveQuest = false;
    [SerializeField] private int questID;
    [SerializeField] private bool isLock = true;
    [SerializeField] private UnityEvent afterFixing;
    [SerializeField] private bool isBomb = false;
    private PlayerController playerController;
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        playerController = FindFirstObjectByType<PlayerController>();
        bool result = PickUpObjList.instance.ExistOnList(idPickUp);
        if (result)
        {
            Destroy(partObj);
        }
            
    }

    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return mainObj;
    }

    public Vector3 GetNearPoint()
    {
        return nearpoint.position;
    }

    public void Use()
    {
        Debug.Log("isLock = " + isLock);
        if(mustHaveQuest)
        {
            if (!QuestController.instance.HaveQuest(questID) && forRepair)
            {
                HUDController.instance.AddConsolelog("It’s broken");
                return;
            }

            if (!QuestController.instance.HaveQuest(questID) && isBomb)
            {
                HUDController.instance.AddConsolelog("You can’t do anything with");
                HUDController.instance.AddConsolelog("this right now");
                return;
            }
        }
        if (isBomb && partObj == null)
        {
            HUDController.instance.AddConsolelog("The bomb has been disarmed");
            return;
        }
        if (forRepair && partObj == null)
        {
            HUDController.instance.AddConsolelog("It works correctly");
            if(afterFixing != null)
                afterFixing.Invoke();
            return;
        }
        if (isLock)
        {
            HUDController.instance.AddConsolelog("You need a wrench");
            return;
        }
        
        if(partObj != null)
        {
            if(isBomb)
            {
                StartCoroutine(DisarmBomb());
                return;
            }
            if(forRepair)
            {
                if(Inventory.instance.PlayerHaveItem(repairPartID))
                {
                    StartCoroutine(Repair());
                }
                else
                {
                    switch (devicesPart)
                    {
                        case DevicesPart.carBattery:
                            HUDController.instance.AddConsolelog("You need a car");
                            HUDController.instance.AddConsolelog("battery");
                            break;
                        case DevicesPart.valve:
                            HUDController.instance.AddConsolelog("You need a valve");
                            break;
                        case DevicesPart.integratedCircuit:
                            HUDController.instance.AddConsolelog("You need an Integrate");
                            HUDController.instance.AddConsolelog("Circuit");
                            break;
                    }
                }
                isLock = true;
                return;
            }
            StartCoroutine(RemovePart());
        }
        else
        {
            HUDController.instance.AddConsolelog("You didn’t remove anything");
        }
        isLock = true;
    }

    public bool CheckKey(int _keyID)
    {
        return keyID == _keyID;
    }

    public void Unlock()
    {
        if (mustHaveQuest)
        {
            if (!QuestController.instance.HaveQuest(questID) && forRepair)
            {
                return;
            }
            if (!QuestController.instance.HaveQuest(questID) && isBomb)
            {
                return;
            }
        }
        isLock = false;
    }

    public bool IsLocked()
    {
        return isLock;
    }

    public IEnumerator Repair()
    {
        yield return new WaitForEndOfFrame();
        playerController.SetBlock(true);
        if (source != null)
        {
            source.Play();
        }
        yield return new WaitForSeconds(1);
        bool canRepair = CanRepair();
        if (canRepair)
        {
            Item item = ItemDB.instance.GetItemById(repairPartID);
            if (item != null)
            {
                SlotItem slot = new SlotItem(item, 1);
                Inventory.instance.RemoveItem(slot);
                PickUpObjList.instance.AddIdToList(idPickUp);
                HUDController.instance.AddConsolelog("You fixed it!");
                Destroy(partObj);
            }
        }
        else
        {
            HUDController.instance.AddConsolelog("You failed to repair it.");
        }
        playerController.SetBlock(false);
        if (canRepair)
        {
            foreach (ActionDialogue action in actions)
            {
                action?.Execute();
            }
        }
    }

    public IEnumerator RemovePart()
    {
        yield return new WaitForEndOfFrame();
        playerController.SetBlock(true);
        if (source != null)
        {
            source.Play();
        }
        yield return new WaitForSeconds(1);
        if (CanRepair())
        {
            switch (devicesPart)
            {
                case DevicesPart.carBattery:
                    HUDController.instance.AddConsolelog("You removed the car");
                    HUDController.instance.AddConsolelog("battery");
                    break;
                case DevicesPart.valve:
                    HUDController.instance.AddConsolelog("You removed the valve");
                    break;
            }
            Destroy(partObj);
            Inventory.instance.AddItem(part);
            PickUpObjList.instance.AddIdToList(idPickUp);
        }
        else
        {
            HUDController.instance.AddConsolelog("Failed to extract any");
            HUDController.instance.AddConsolelog("parts");
        }
        playerController.SetBlock(false);
    }

    public IEnumerator DisarmBomb()
    {
        yield return new WaitForEndOfFrame();
        playerController.SetBlock(true);
        if (source != null)
        {
            source.Play();
        }
        yield return new WaitForSeconds(1);
        if(PlayerStats.instance.GetTechnical() > 6)
        {
            HUDController.instance.AddConsolelog("The bomb has been disarmed");
            HUDController.instance.AddConsolelog("You added a Nuclear");
            HUDController.instance.AddConsolelog("Battery");
            Destroy(partObj);
            Inventory.instance.AddItem(part);
            PickUpObjList.instance.AddIdToList(idPickUp);
            foreach (ActionDialogue action in actions)
            {
                action?.Execute();
            }
        }
        else
        {
            HUDController.instance.AddConsolelog("Disarm attempt failed");
        }
        playerController.SetBlock(false);
    }

    public bool CanRepair()
    {
        int hitChance = Random.Range(0, 10000) % 100;
        Debug.Log($"Hit Chance: {hitChance} Repair Chance: {PlayerStats.instance.GetRepairChance()}");
        if (hitChance >= PlayerStats.instance.GetRepairChance())
        {
            return false;
        }
        return true;
    }
}

public enum DevicesPart
{
    carBattery, valve, integratedCircuit
}
