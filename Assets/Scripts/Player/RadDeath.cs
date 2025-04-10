using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RadDeath : MonoBehaviour
{
    public static RadDeath instance;
    [SerializeField] private bool active = true;
    [SerializeField] private string deathSceneName = "DeathScene";
    private PlayerController player;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        active = true;
        player = GetComponent<PlayerController>();
    }

    public void SetDeath()
    {
        if (!active)
            return;
        active = false;
        PlayerStats.instance.RemoveHealthPoint(1000);
        player.SetBlock(true);
        StartCoroutine(AfterDeath());
    }

    private IEnumerator AfterDeath()
    {
        player.SetDeathState();
        yield return new WaitForSeconds(2f);
        Vector3 spawnBlood = player.transform.position - player.transform.forward * 1f;
        spawnBlood.y = 0.01f;
        GameObject bloodPrefab = CombatController.instance.GetBloodPrefab();
        Instantiate(bloodPrefab, spawnBlood, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        FadeController.instance.SetFadeIn(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(deathSceneName);
    }
}
