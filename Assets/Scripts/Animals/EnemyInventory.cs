using UnityEngine;

public class EnemyInventory : MonoBehaviour, IUsableObj
{
    private EnemyController enemy;
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
        return transform.position;
    }

    public void Use()
    {
        enemy = GetComponent<EnemyController>();
        CabinetUI.instance.Show(enemy.GetItems(), enemy.GetNameEnemy());
    }

}
