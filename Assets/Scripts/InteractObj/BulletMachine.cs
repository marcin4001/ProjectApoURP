using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletMachine : MonoBehaviour, IUsableObj
{
    public Transform nearPoint;
    [SerializeField] private SlotItem money;
    [SerializeField] private List<SlotItem> items = new List<SlotItem>();
    [SerializeField] private AudioClip clip;
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
        if (!Inventory.instance.PlayerHaveItem(money))
        {
            HUDController.instance.AddConsolelog("You don’t have enough");
            HUDController.instance.AddConsolelog("money.");
            return;
        }
        StartCoroutine(Doing());
    }

    private IEnumerator Doing()
    {
        yield return new WaitForEndOfFrame();
        CameraMovement.instance.SetBlock(true);
        player.SetBlock(true);
        if (source != null)
        {
            source.clip = clip;
            source.Play();
        }
        yield return new WaitForSeconds(1.7f);
        CameraMovement.instance.SetBlock(false);
        player.SetBlock(false);
        int randomItem = Random.Range(0, items.Count);
        Inventory.instance.AddItem(items[randomItem]);
        MiscItem miscItem = (MiscItem)items[randomItem].GetItem();
        HUDController.instance.AddConsolelog("You got 1 random round");
        if (miscItem.isAmmo)
        {
            WeaponObject weapon = WeaponController.instance.GetWeaponByAmmo(items[randomItem].GetItem().id);
            if (weapon != null)
                weapon.UpdateAmmoSlot();
        }
        Inventory.instance.RemoveItem(money);
    }
}
