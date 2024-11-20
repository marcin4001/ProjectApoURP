using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    [SerializeField] private EnemyGroup group;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.StopMove();
            CombatController.instance.SetGroup(group);
            CombatController.instance.StartCombat(true);
            Destroy(gameObject);
        }
    }
}
