using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private EnemyController[] enemies;
    [SerializeField] private PlayerController player;
    private float minDistance = 4f;

    //private void Start()
    //{
    //    StartCoroutine(UpdateGroup());
    //}

    private void OnEnable()
    {
        StartCoroutine(UpdateGroup());
    }

    private IEnumerator UpdateGroup()
    {
        player = FindFirstObjectByType<PlayerController>();
        yield return new WaitForSeconds(0.1f);
        
        List<EnemyController> temp = new List<EnemyController>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                temp.Add(enemies[i]);
            }
        }
        Debug.Log(temp.Count);
        enemies = temp.ToArray();
        if (gameObject.activeSelf)
            StartCoroutine(Trigger());
        
    }

    private IEnumerator Trigger()
    {
        bool stop = false;
        while (!stop)
        {
            if(GameParam.instance.inCombat)
                yield break;
            foreach (EnemyController enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

                if (distance < minDistance)
                {
                    stop = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Player is near");
        player.StopMove();
        yield return new WaitForEndOfFrame();
        CombatController.instance.SetGroup(this);
        CombatController.instance.StartCombat(true);
    }

    public EnemyController[] GetEnemies()
    {
        return enemies;
    }
}
