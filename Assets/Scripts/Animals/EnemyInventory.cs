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
        enemy = GetComponent<EnemyController>();
        return enemy.GetNearPoint();
    }

    public void Use()
    {
        enemy = GetComponent<EnemyController>();
        CabinetUI.instance.Show(enemy.GetItems(), enemy.GetNameEnemy());
    }

}
