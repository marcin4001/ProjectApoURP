using System.Collections.Generic;
using UnityEngine;

public class FarmRestorer : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private GameObject carBattery;
    [SerializeField] private bool inLab;
    private PlayerController player;

    private void Start()
    {
        if(inLab)
            return;
        player = FindFirstObjectByType<PlayerController>();
        if(carBattery != null )
        {
            carBattery.SetActive(false);
        }
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
        if (inLab)
        {
            HUDController.instance.AddConsolelog("I can’t take the Farm");
            HUDController.instance.AddConsolelog("Restorer. It’s");
            HUDController.instance.AddConsolelog("incomplete.");
            return;
        }
    }
}
