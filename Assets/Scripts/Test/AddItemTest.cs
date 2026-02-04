using System.Collections.Generic;
using UnityEngine;

public class AddItemTest : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameParam.instance.inDev)
            return;
        if(Input.GetKeyUp(KeyCode.I))
        {
            foreach(Item item in items)
            {
                if(item == null) return;
                SlotItem slot = new SlotItem(item, 1);
                Inventory.instance.AddItem(slot);
            }
        }
    }
}
