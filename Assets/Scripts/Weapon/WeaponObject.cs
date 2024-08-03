using System.Collections;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [SerializeField] private WeaponItem weapon;
    [SerializeField] private ParticleSystem muzzle;
    [SerializeField] private float startPlayMuzzle = 0.25f;
    [SerializeField] private int currentAmmoInGun = 0;
    [SerializeField] private int ammoOutGun = 0;

    private void Start()
    {
        //Test 
        currentAmmoInGun = weapon.ammoMax;
        ammoOutGun = weapon.ammoMax;
    }

    public int GetIdItem()
    {
        if(weapon == null)
            return -1;
        return weapon.id;
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
        ShowAmmoInConsole();
    }

    public void Reload()
    {
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
