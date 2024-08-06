using System.Collections;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [SerializeField] private WeaponItem weapon;
    [SerializeField] private ParticleSystem muzzle;
    [SerializeField] private float startPlayMuzzle = 0.25f;
    [SerializeField] private int currentAmmoInGun = 0;
    [SerializeField] private int ammoOutGun = 0;
    [SerializeField] private SlotItem ammoSlot;

    private void Start()
    {

    }

    public void InitAmmo()
    {
        if (weapon.type == WeaponType.Melee)
            return;
        if (ammoSlot != null && !ammoSlot.IsEmpty())
            return;
        ammoSlot = Inventory.instance.GetSlot(weapon.idAmmo);
        if (ammoSlot.GetAmount() <= weapon.ammoMax)
        {
            currentAmmoInGun = ammoSlot.GetAmount();
        }
        else
        {
            currentAmmoInGun = weapon.ammoMax;
            ammoOutGun = ammoSlot.GetAmount() - currentAmmoInGun;
        }
    }

    public void UpdateAmmoOutGun()
    {
        if (ammoSlot == null && ammoSlot.IsEmpty())
            return;
        if (currentAmmoInGun == 0 && ammoOutGun == 0)
            return;
        Debug.Log("Doszło");
        ammoOutGun = ammoSlot.GetAmount() - currentAmmoInGun;

    }

    public int GetIdItem()
    {
        if(weapon == null)
            return -1;
        return weapon.id;
    }

    public int GetIdAmmo()
    {
        if (weapon == null)
            return -1;
        return weapon.idAmmo;
    }

    public bool IsMelee()
    {
        if(weapon == null) return true;
        return weapon.type == WeaponType.Melee;
    }

    public bool OutOfAmmo()
    {
        if(currentAmmoInGun <= 0)
            HUDController.instance.AddConsolelog("Out of ammo!");
        return currentAmmoInGun <= 0;
    }

    public bool IsFull()
    {
        if(weapon == null) return false;
        return currentAmmoInGun >= weapon.ammoMax;
    }

    public void StartPlayMuzzle()
    {
        StartCoroutine(PlayMuzzle());
    }

    private IEnumerator PlayMuzzle()
    {
        yield return new WaitForSeconds(startPlayMuzzle);
        if(muzzle != null)
            muzzle.Play();
    }

    public void RemoveAmmo(int ammo)
    {
        currentAmmoInGun -= ammo;
        int ammoAmount = ammoSlot.GetAmount() - ammo;
        if (ammoAmount <= 0)
        {
            Inventory.instance.RemoveItem(ammoSlot);
            ammoSlot = new SlotItem(null, 0);
            ShowAmmoInConsole();
            return;
        }
        ammoSlot.SetAmount(ammoAmount);
        ShowAmmoInConsole();
    }

    public void Reload()
    {
        if(ammoSlot == null || ammoSlot.IsEmpty())
        {
            ammoSlot = Inventory.instance.GetSlot(weapon.idAmmo);
            ammoOutGun = ammoSlot.GetAmount();
        }
        int neededAmmo = weapon.ammoMax - currentAmmoInGun;
        if(neededAmmo <= ammoOutGun)
        {
            currentAmmoInGun += neededAmmo;
            ammoOutGun -= neededAmmo;
        }
        else
        {
            currentAmmoInGun += ammoOutGun;
            ammoOutGun = 0;
        }
        ShowAmmoInConsole();
    }

    public void ShowAmmoInConsole()
    {
        HUDController.instance.AddConsolelog($"Ammo: {currentAmmoInGun}/{ammoOutGun}");
    }
}
