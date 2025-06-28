using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneStartName = "MainScene";
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private int indexTheme = 0;
    void Start()
    {
        MusicManager.instance.SetMaxVolume(GameParam.instance.maxVolumeTheme);
        MusicManager.instance.SetTheme(indexTheme);
        playButton.onClick.AddListener(OnClickPlay);
        quitButton.onClick.AddListener(OnClickQuit);
    }

    private void OnClickPlay()
    {
        GameParam.instance.SetStartParam();
        if(Inventory.instance != null)
            Inventory.instance.Clear();
        //PickUpObjList.instance.CopyList();
        PickUpObjList.instance.Clear();
        KilledEnemiesList.instance.ClearList();
        NPCObjList.instance.ClearList();
        ListCabinet.instance.CopyList();
        ListOffers.instance.CopyList();
        QuestController.instance.ClearList();
        SceneManager.LoadScene(sceneStartName);
    }

    private void OnClickQuit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
