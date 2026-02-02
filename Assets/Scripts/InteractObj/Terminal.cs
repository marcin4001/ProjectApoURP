using System.Collections;
using UnityEngine;

public class Terminal : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private MetalDoor metalDoor;
    [SerializeField] private int technicalReq = 8;
    [SerializeField] private int idQuest;
    void Start()
    {
        StartCoroutine(CheckDoor());
    }

    private IEnumerator CheckDoor()
    {
        yield return new WaitForSeconds(0.1f);
        if(metalDoor == null)
            Destroy(this);
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
        if(PlayerStats.instance.GetTechnical() != technicalReq)
        {
            HUDController.instance.AddConsolelog("Failed to open the door");
            HUDController.instance.AddConsolelog("remotely");
            HUDController.instance.AddConsolelog($"(Required Technical: {technicalReq})");
            return;
        }
        HUDController.instance.AddConsolelog("You opened the door");
        HUDController.instance.AddConsolelog("remotely");
        if(metalDoor != null)
            metalDoor.OpenDoorTerminal();
        QuestController.instance.SetComplete(idQuest);
        Destroy(this);
    }
}
