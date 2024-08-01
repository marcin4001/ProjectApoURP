using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] WeaponObject currentWeapon;
    [SerializeField] private List<WeaponObject> weaponItems = new List<WeaponObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
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

    public void SetCurrentWeapon(Item item)
    {
        currentWeapon = GetWeapon(item.id);
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
}
