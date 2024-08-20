using System.Collections;
using UnityEngine;

public class Stove : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private Item rawMeatItem;
    [SerializeField] private Item porkChop;
    [SerializeField] private GameObject pan;
    [SerializeField] private float cookingTime = 2f;
    private PlayerController player;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        pan.SetActive(false);
    }

    public bool CanUse()
    {
        if(Inventory.instance.PlayerHaveItem(rawMeatItem.id))
            return true;
        else
            return false;
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
        StartCoroutine(Cooking());
    }

    private IEnumerator Cooking()
    {
        pan.SetActive(true);
        CameraMovement.instance.SetBlock(true);
        player.SetBlock(true);
        SlotItem rawMeatSlot = new SlotItem(rawMeatItem, 1);
        Inventory.instance.RemoveItem(rawMeatSlot);
        yield return new WaitForSeconds(cookingTime);
        pan.SetActive(false);
        SlotItem porkChopSlot = new SlotItem(porkChop, 1);
        Inventory.instance.AddItem(porkChopSlot);
        CameraMovement.instance.SetBlock(false);
        player.SetBlock(false);
    }
}
