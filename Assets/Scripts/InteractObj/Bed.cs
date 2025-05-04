using System.Collections;
using UnityEngine;

public class Bed : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private int cost = 0;
    private int idMoney = 202;
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
        if (cost <= 0)
        {
            StartCoroutine(Sleeping());
            return;
        }
        else
        {
            Item dollar = ItemDB.instance.GetItemById(idMoney);
            SlotItem slotMoney = new SlotItem(dollar, cost); 
            if(!Inventory.instance.PlayerHaveItem(slotMoney))
            {
                HUDController.instance.AddConsolelog("You don’t have enough");
                HUDController.instance.AddConsolelog("money to use the bed.");
                player.SetBlock(false);
                return;
            }
            Inventory.instance.RemoveItem(slotMoney);
            StartCoroutine(SleepingLong());
        }
    }

    private IEnumerator Sleeping()
    {
        CameraMovement.instance.SetBlock(true);
        FadeController.instance.SetFadeIn(true);
        player.SetBlock(true);
        yield return new WaitForSeconds(2f);
        TimeGame.instance.AddHours(6);
        FadeController.instance.SetFadeIn(false);
        yield return new WaitForSeconds(1.5f);
        CameraMovement.instance.SetBlock(false);
        player.SetBlock(false);
    }

    private IEnumerator SleepingLong()
    {
        CameraMovement.instance.SetBlock(true);
        FadeController.instance.SetFadeIn(true);
        player.SetBlock(true);
        yield return new WaitForSeconds(2f);
        TimeGame.instance.AddHours(12);
        FadeController.instance.SetFadeIn(false);
        yield return new WaitForSeconds(1.5f);
        CameraMovement.instance.SetBlock(false);
        player.SetBlock(false);
    }
}
