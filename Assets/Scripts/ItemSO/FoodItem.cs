using UnityEngine;

[CreateAssetMenu(fileName = "FoodItem", menuName = "Item/FoodItem")]
public class FoodItem : Item
{
    public int healPoint = 10;
    public GameObject spawnObj;
}
