using UnityEngine;

public class AchievementTrigger : MonoBehaviour
{
    [SerializeField] private string idAchievement;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SteamAchievements.Add(idAchievement);
            Destroy(gameObject);
        }
    }
}
