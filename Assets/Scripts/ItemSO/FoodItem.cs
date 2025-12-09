using UnityEngine;
using static UnityEngine.Splines.SplineInstantiate;

[CreateAssetMenu(fileName = "FoodItem", menuName = "Item/FoodItem")]
public class FoodItem : Item
{
    public int healPoint = 10;
    public bool radioactive = false;
    public bool healRadioactive = false;
    public bool healRadioactiveDuo = false;
    public bool isDrink = false;
    public bool isSpicy = false;
    public GameObject spawnObj;
    public bool returnObj;
    public int idObj;
}
