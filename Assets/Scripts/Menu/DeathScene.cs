using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    [SerializeField] private float timeLoadScene = 4f;
    [SerializeField] private bool anyKeyTrigger = false;
    [SerializeField] private bool startAchievement = false;
    [SerializeField] private string idAchievement;
    [SerializeField] private TextMeshProUGUI infoText;
    private LocalizationCollector loc;
    void Start()
    {
        if (anyKeyTrigger)
        {
            StartCoroutine(AnyKeyTrigger());
        }
        else
        {
            StartCoroutine(LoadMenuScene());
        }
        if (startAchievement)
            SteamAchievements.Add(idAchievement);
        loc = new LocalizationCollector();
        if(infoText != null)
        {
            string info = "Press any key to return to the menu";
            infoText.text = loc.LoadTranslate(info);
        }
    }

    private IEnumerator LoadMenuScene()
    {
        yield return new WaitForSeconds(timeLoadScene);
        SceneManager.LoadScene(0);
    }

    private IEnumerator AnyKeyTrigger()
    {
        yield return new WaitForSeconds(1.5f);
        while(true)
        {
            if(Input.anyKey)
            {
                SceneManager.LoadScene(0);
            }
            yield return new WaitForEndOfFrame();
        }
        
    }
}
