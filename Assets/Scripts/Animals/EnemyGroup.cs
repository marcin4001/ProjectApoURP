using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private EnemyController[] enemies;

    private void Start()
    {
        StartCoroutine(UpdateGroup());
    }

    private IEnumerator UpdateGroup()
    {
        yield return new WaitForSeconds(0.1f);
        List<EnemyController> temp = new List<EnemyController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                Debug.Log(enemies[i]);
                temp.Add(enemies[i]);
            }
        }
        Debug.Log(temp.Count);
        enemies = temp.ToArray();
    }

    public EnemyController[] GetEnemies()
    {
        return enemies;
    }
}
