using UnityEngine;

public class Cactus : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private SlotItem flower;
    [SerializeField] private GameObject flowerObj;
    [SerializeField] private int idPickUp;
    private bool active = true;
    void Start()
    {
        bool result = PickUpObjList.instance.ExistOnList(idPickUp);
        if(result)
        {
            active = false;
            flowerObj.SetActive(false);
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
        if(active)
        {
            active = false;
            flowerObj.SetActive(false);
            Inventory.instance.AddItem(flower);
            PickUpObjList.instance.AddIdToList(idPickUp);
        }
    }
}
