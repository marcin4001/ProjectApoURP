using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController instance;
    [SerializeField] WeaponObject currentWeapon;
    [SerializeField] private List<WeaponObject> weaponItems = new List<WeaponObject>();
    [SerializeField] private AudioSource handSource;
    [SerializeField] private AudioClip punchClip;
    [SerializeField] private float startPlayAudio = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        HideAllWeapons();
    }

    public void ShowWeapon(int id)
    {
        HideAllWeapons();
        WeaponObject weapon = weaponItems.Find(x => x.GetIdItem() == id);
        if (weapon != null)
        {
            weapon.gameObject.SetActive(true);
        }
    }

    public WeaponObject GetWeapon(int id)
    {
        WeaponObject weapon = weaponItems.Find(x => x.GetIdItem() == id);
        if (weapon != null)
        {
            return weapon;
        }
        else
        {
            return null;
        }
    }

    public WeaponObject GetWeaponByAmmo(int idAmmo)
    {
        WeaponObject weapon = weaponItems.Find(x => x.GetIdAmmo() == idAmmo);
        if (weapon != null)
        {
            return weapon;
        }
        else
        {
            return null;
        }
    }

    public void SetCurrentWeapon(Item item)
    {
        currentWeapon = GetWeapon(item.id);
        currentWeapon.InitAmmo();
        if (currentWeapon != null && !currentWeapon.IsMelee())
        {
            currentWeapon.ShowAmmoInConsole();
        }
    }

    public void ShowCurrentWeapon(bool value)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(value);
        }
    }

    public void SetEmptyCurrentWeapon()
    {
        currentWeapon = null;
        HideAllWeapons();
    }

    public WeaponObject GetCurrentWeapon()
    {
        return currentWeapon;
    }
    private void HideAllWeapons()
    {
        foreach (WeaponObject weapon in weaponItems)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    public void StartPlayPunch()
    {
        StartCoroutine(PlayPunch());
    }

    private IEnumerator PlayPunch()
    {
        yield return new WaitForSeconds(startPlayAudio);
        if (handSource != null)
            handSource.PlayOneShot(punchClip);
    }
}
