using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool isLock = true;
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
        if (forRepair && partObj == null)
        {
            HUDController.instance.AddConsolelog("It works correctly");
            return;
        }
        if (isLock)
        {
            HUDController.instance.AddConsolelog("You need a wrench");
            return;
        }
        
        if(partObj != null)
        {
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
        Item item = ItemDB.instance.GetItemById(repairPartID);
        if (item != null)
        {
            SlotItem slot = new SlotItem(item, 1);
            Inventory.instance.RemoveItem(slot);
            PickUpObjList.instance.AddIdToList(idPickUp);
            HUDController.instance.AddConsolelog("You fixed it!");
            Destroy(partObj);
        }
        playerController.SetBlock(false);
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
        playerController.SetBlock(false);
    }
}

public enum DevicesPart
{
    carBattery, valve
}
