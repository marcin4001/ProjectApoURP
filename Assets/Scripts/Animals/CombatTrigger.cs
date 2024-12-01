using System.Collections;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    [SerializeField] private EnemyGroup group;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(StartCombat(other));
        }
    }

    private IEnumerator StartCombat(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        player.StopMove();
        yield return null;
        CombatController.instance.SetGroup(group);
        CombatController.instance.StartCombat(true);
        Destroy(gameObject);
    }
}
