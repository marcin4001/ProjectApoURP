using System.Collections;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [SerializeField] private WeaponItem weapon;
    [SerializeField] private ParticleSystem muzzle;
    [SerializeField] private float startPlayMuzzle = 0.25f;
    [SerializeField] private float startPlayAudio;
    [SerializeField] private int currentAmmoInGun = 0;
    [SerializeField] private int ammoOutGun = 0;
    [SerializeField] private SlotItem ammoSlot;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip reloadSound;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void InitAmmo()
    {
        
        if (weapon.type == WeaponType.Melee)
            return;
        if (ammoSlot != null && !ammoSlot.IsEmpty())
        {
            if(ammoSlot.GetAmount() < currentAmmoInGun)
                currentAmmoInGun = ammoSlot.GetAmount();
            return;
        }
        ammoSlot = Inventory.instance.GetSlot(weapon.idAmmo);
        //if(ammoSlot == null || ammoSlot.IsEmpty())
        //    ammoSlot = HUDController.instance.GetSlotById(weapon.idAmmo);
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
        ammoOutGun = ammoSlot.GetAmount() - currentAmmoInGun;
    }

    public void SetEmptyAmmo()
    {
        ammoSlot = new SlotItem(null, 0);
        currentAmmoInGun = 0;
        ammoOutGun = 0;
    }

    public void UpdateAmmoSlot()
    {
        ammoSlot = Inventory.instance.GetSlot(weapon.idAmmo);
        if(ammoSlot == null ||  ammoSlot.IsEmpty())
        {
            SetEmptyAmmo();
            return;
        }
        int fullAmmo = ammoSlot.GetAmount();
        if(fullAmmo > currentAmmoInGun)
        {
            ammoOutGun = fullAmmo - currentAmmoInGun;
        }
        else
        {
            currentAmmoInGun = fullAmmo;
            ammoOutGun = 0;
        }
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

    public float GetRange()
    {
        if(weapon == null)
            return 0f;
        return weapon.range;
    }

    public int GetDamage()
    {
        if(weapon == null)
            return 5;
        return weapon.damage;
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

    public void StartPlayAttack()
    {
        StartCoroutine(PlayAttackSound());
    }

    private IEnumerator PlayAttackSound()
    {
        yield return new WaitForSeconds(startPlayAudio);
        if (source != null)
            source.PlayOneShot(attackClip);
    }

    public void PlayReloadSound()
    {
        if (source != null)
            source.PlayOneShot(reloadSound);
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
