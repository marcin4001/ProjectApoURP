using UnityEngine;

public class EnemySpawnerGroup : MonoBehaviour
{
    [SerializeField] private EnemyGroup[] enemyGroups;
    void Start()
    {
        if(enemyGroups.Length == 0)
            return;
        foreach(EnemyGroup group in enemyGroups)
            group.gameObject.SetActive(false);
        int groupCount = enemyGroups.Length;
        int probability = 100 / groupCount;
        int randomVal = Random.Range(0, 101);
        Debug.Log($"Random Val: {randomVal}");
        int cumulativeProbability = 0;

        for(int i = 0; i < groupCount; i++)
        {
            cumulativeProbability += probability;

            if(randomVal <= cumulativeProbability)
            {
                enemyGroups[i].gameObject.SetActive(true);
                Debug.Log($"Active Group: {i}");
                return;
            }
        }
        enemyGroups[groupCount - 1].gameObject.SetActive(true);
        Debug.Log($"Fallback Active Group: {groupCount - 1}");
    }

}
