using UnityEngine;

public class PlayerClothes : MonoBehaviour
{
    public static PlayerClothes instance;
    [SerializeField] private Renderer top;
    [SerializeField] private Renderer bottom;
    [SerializeField] private GameObject helmet;
    [SerializeField] private GameObject redCap;
    [SerializeField] private Material defaultTopMaterial;
    [SerializeField] private Material defaultBottomMaterial;
    [SerializeField] private ArmorItem armorItem;
    private void Awake()
    {
        instance = this;
        defaultTopMaterial = top.material;
        defaultBottomMaterial = bottom.material;
        helmet.SetActive(false);
        redCap.SetActive(false);
    }
    void Start()
    {
        
    }

    public void SetClothes(ArmorItem armor)
    {
        if(armor == null)
            return;
        armorItem = armor;
        if (armor.clothes_top != null)
        {
            top.material = armor.clothes_top;
        }
        if (armor.clothes_bottom != null)
        {
            bottom.material = armor.clothes_bottom;
        }
        if(armor.headgear == Headgear.Helmet)
        {
            helmet.SetActive(true);
        }
        if(armor.headgear == Headgear.RedCap)
        {
            redCap.SetActive(true);
        }
    }

    public void ResetClothes()
    {
        top.material = defaultTopMaterial;
        bottom.material = defaultBottomMaterial;
        helmet.SetActive(false);
        redCap.SetActive(false);
        armorItem = null;
    }

    public int GetDefence()
    {
        if(armorItem == null)
            return 0;
        return armorItem.defense;
    }
}
