using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneDialogue : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private PlayerController playerController;
    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void Load()
    {
        StartCoroutine(LoadingScene());
    }

    private IEnumerator LoadingScene()
    {
        yield return new WaitForEndOfFrame();
        CameraMovement.instance.SetBlock(true);
        FadeController.instance.SetFadeIn(true);
        playerController.SetBlock(true);
        yield return new WaitForSeconds(2f);
        GameParam.instance.UpdateParam();
        if (ListCabinet.instance != null)
            ListCabinet.instance.SaveCabinets();
        if (ListOffers.instance != null)
            ListOffers.instance.SaveOffers();
        SceneManager.LoadScene(sceneName);
    }

}
