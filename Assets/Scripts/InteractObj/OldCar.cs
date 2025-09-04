using UnityEngine;
using UnityEngine.SceneManagement;

public class OldCar : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearpoint;
    [SerializeField] private GameObject mainObj;
    [SerializeField] private int keyID;
    [SerializeField] private SlotItem carBattery;
    [SerializeField] private GameObject carBatteryObj;
    [SerializeField] private int idPickUp;
    private bool isLock = true;
    private PlayerController playerController;
    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        bool result = PickUpObjList.instance.ExistOnList(idPickUp);
        if (result)
        {
            Destroy(carBatteryObj);
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
        
        if(carBatteryObj != null)
        {
            HUDController.instance.AddConsolelog("You removed the car");
            HUDController.instance.AddConsolelog("battery");
            Destroy(carBatteryObj);
            Inventory.instance.AddItem(carBattery);
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
