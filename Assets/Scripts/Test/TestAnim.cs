using UnityEngine;

public class TestAnim : MonoBehaviour
{
    [SerializeField] private AnimationPlayer anim;
    [SerializeField] private WeaponObject weapon;
    [SerializeField] private WeaponType weaponType = WeaponType.Melee;
    void Start()
    {
        anim = GetComponent<AnimationPlayer>(); 
        switch(weaponType)
        {
            case WeaponType.Melee:
                anim.ActiveBaseLayer();
                break;
            case WeaponType.HandGun:
                anim.ActiveHandGunLayer();
                break;
            case WeaponType.Rifle:
                anim.ActiveRifleLayer();
                break;
        }
    }


    void Update()
    {
        if(Input.GetKeyUp(KeyCode.F1))
        {
            anim.Shot();
            weapon.StartPlayAttack();
            weapon.StartPlayMuzzle();
        }
        if(Input.GetKeyUp(KeyCode.F2))
        {
            anim.Reload();
            weapon.PlayReloadSound();
        }
    }
}
