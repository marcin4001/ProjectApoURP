using UnityEngine;

public class PlayerClothes : MonoBehaviour
{
    public static PlayerClothes instance;
    [SerializeField] private Renderer top;
    [SerializeField] private Renderer bottom;
    [SerializeField] private Material defaultTopMaterial;
    [SerializeField] private Material defaultBottomMaterial;
    private void Awake()
    {
        instance = this;
        defaultTopMaterial = top.material;
        defaultBottomMaterial = bottom.material;
    }
    void Start()
    {
        
    }

    public void SetClothes(ArmorItem armor)
    {
        if(armor == null)
            return;
        if (armor.clothes_top != null)
        {
            top.material = armor.clothes_top;
        }
        if (armor.clothes_bottom != null)
        {
            bottom.material = armor.clothes_bottom;
        }
    }

    public void ResetClothes()
    {
        top.material = defaultTopMaterial;
        bottom.material = defaultBottomMaterial;
    }
}
