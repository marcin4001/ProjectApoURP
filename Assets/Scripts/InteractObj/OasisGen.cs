using System.Collections.Generic;
using UnityEngine;

public class OasisGen : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
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
        HUDController.instance.AddConsolelog("I can’t take the oasis");
        HUDController.instance.AddConsolelog("generator. It’s");
        HUDController.instance.AddConsolelog("incomplete.");
    }
}
