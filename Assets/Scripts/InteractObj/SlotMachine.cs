using System.Collections;
using UnityEngine;

public class SlotMachine : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private Animator anim;
    [SerializeField] private string actionParam = "Action";
    [SerializeField] private bool active = false;
    private PlayerController player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }
    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public Vector3 GetNearPoint()
    {
        return nearPoint.position;
    }

    public void Use()
    {
        if(active)
            return;
        Item dollar = ItemDB.instance.GetItemById(202);
        SlotItem money = new SlotItem(dollar, 5);
        if (!Inventory.instance.PlayerHaveItem(money))
        {
            HUDController.instance.AddConsolelog("You don’t have enough");
            HUDController.instance.AddConsolelog("money to use the slot");
            HUDController.instance.AddConsolelog("machine.");
            player.SetBlock(false);
            return;
        }
        StartCoroutine(DrawSlot(money));
    }

    private IEnumerator DrawSlot(SlotItem slotMoney)
    {
        yield return new WaitForEndOfFrame();
        player.SetBlock(true);
        active = true;
        anim.SetTrigger(actionParam);
        yield return new WaitForSeconds(0.5f);
        SlotMachineUI.instance.Draw(slotMoney);
        yield return new WaitForSeconds(5f);
        player.SetBlock(false);
        active = false;
    }
}
