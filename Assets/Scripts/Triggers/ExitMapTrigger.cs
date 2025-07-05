using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMapTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName = "DemoMenu";
    [SerializeField] private bool startQuest = false;
    [SerializeField] private bool endDemo = false;
    [SerializeField] private int idQuest = 0;
    [SerializeField] private string textLog = "";
    [SerializeField] private string thxDemoScene = "ThxDemoScene";
    private PlayerController playerController;
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(startQuest)
        {
            if (!QuestController.instance.HaveQuest(idQuest))
            {
                HUDController.instance.AddConsolelog(textLog);
                return;
            }
        }
        if(GameParam.instance.inDemo && endDemo)
        {
            if(QuestController.instance.HaveQuest(idQuest))
            {
                StartCoroutine(EndDemoLoadScene());
                return;
            }
        }
        if (other.tag == "Player")
        {
            playerController.SetBlock(true);
            StartCoroutine(LoadScene());
        }
    }

    private IEnumerator LoadScene()
    {
        if(CombatController.instance != null)
        {
            CombatController.instance.StopCombat();
        }
        yield return new WaitForSeconds(2);
        if (GameParam.instance != null)
        {
            GameParam.instance.UpdateParam();
            GameParam.instance.prevScene = SceneManager.GetActiveScene().name;
        }
        if(ListCabinet.instance != null)
            ListCabinet.instance.SaveCabinets();
        if(ListOffers.instance != null)
            ListOffers.instance.SaveOffers();
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator EndDemoLoadScene()
    {
        playerController.SetBlock(true);
        if (CombatController.instance != null)
        {
            CombatController.instance.StopCombat();
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(thxDemoScene);
    }
}
