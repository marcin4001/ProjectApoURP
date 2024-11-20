using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private EnemyController[] enemies;
    
    public EnemyController[] GetEnemies()
    {
        return enemies;
    }
}
