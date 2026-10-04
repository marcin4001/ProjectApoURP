using UnityEngine;

public class AddAllItemsToInv : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ItemDB.instance.AddAllItemToInventory();
        }
    }
}
