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
    private bool isLock = true;
    private PlayerController playerController;
    private void Start()
    {
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
        if (isLock)
        {
            HUDController.instance.AddConsolelog("You need a wrench");
            return;
        }
        
        if(partObj != null)
        {
            switch(devicesPart)
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
}

public enum DevicesPart
{
    carBattery, valve
}
