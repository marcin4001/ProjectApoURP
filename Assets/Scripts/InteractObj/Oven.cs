using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private List<SlotItem> ingredients = new List<SlotItem>();
    [SerializeField] private SlotItem product;
    [SerializeField] private float bakingTime = 2f;
    private AudioSource source;
    private PlayerController player;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        player = FindFirstObjectByType<PlayerController>();
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
        foreach (SlotItem item in ingredients)
        {
            if(!Inventory.instance.PlayerHaveItem(item))
            {
                HUDController.instance.AddConsolelog("You don't have enough");
                HUDController.instance.AddConsolelog("ingredients for the pizza");
                return;
            }
        }
        StartCoroutine(Baking());
    }

    private IEnumerator Baking()
    {
        yield return new WaitForEndOfFrame();
        foreach (SlotItem item in ingredients)
        {
            Inventory.instance.RemoveItem(item);
        }
        CameraMovement.instance.SetBlock(true);
        player.SetBlock(true);
        if (source != null)
            source.Play();
        yield return new WaitForSeconds(bakingTime);
        Inventory.instance.AddItem(product);
        CameraMovement.instance.SetBlock(false);
        player.SetBlock(false);
        if (source != null)
            source.Stop();
    }
}
