using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMapTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName = "DemoMenu";
    private PlayerController playerController;
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
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
}
