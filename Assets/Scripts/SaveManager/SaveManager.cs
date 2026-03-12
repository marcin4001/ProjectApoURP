using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private PlayerController playerController;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Save();
        }
    }

    public void Save()
    {
        string folderPath = Application.persistentDataPath + "/Save";
        
        if(!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        //GameParam
        GameParam.instance.UpdateParam();
        GameParam.instance.prevScene = SceneManager.GetActiveScene().name;
        GameParam.instance.playerPositionSave = playerController.transform.position;
        GameParam.instance.playerRotationSave = playerController.transform.eulerAngles;
        string gameParamSave = JsonUtility.ToJson(GameParam.instance, true);
        File.WriteAllText(folderPath + "/GameParam.json", gameParamSave);

        //NPCList
        NPCObjListSave nPCObjListSave = NPCObjList.instance.GetSaveObj();
        string npcSave = JsonUtility.ToJson(nPCObjListSave, true);
        File.WriteAllText(folderPath + "/NPCList.json", npcSave);

        //ListCabinet
        ListCabinet.instance.SaveCabinets();
        ListCabinetSave listCabinetSave = ListCabinet.instance.GetSaveObj();
        string caninetSave = JsonUtility.ToJson(listCabinetSave, true);
        File.WriteAllText(folderPath + "/CabinetList.json", caninetSave);

        //PickUpObjList
        PickUpObjListSave pickUpObjListSave = PickUpObjList.instance.GetSaveObj();
        string pickUpObjSave = JsonUtility.ToJson(pickUpObjListSave, true);
        File.WriteAllText(folderPath + "/PickUpObjList.json", pickUpObjSave);

        //ListOffers
        ListOffers.instance.SaveOffers();
        ListOffersSave listOffersSave = ListOffers.instance.GetSaveObj();
        string offersSave = JsonUtility.ToJson(listOffersSave, true);
        File.WriteAllText(folderPath + "/ListOffers.json", offersSave);

        //QuestController
        QuestListSave questListSave = QuestController.instance.GetSaveObj();
        string questsSave = JsonUtility.ToJson(questListSave, true);
        File.WriteAllText(folderPath + "/QuestList.json", questsSave);

        //KilledEnemiesList
        KilledEnemiesListSave killedEnemiesListSave = KilledEnemiesList.instance.GetSaveObj();
        string killedEnemiesSave = JsonUtility.ToJson(killedEnemiesListSave, true);
        File.WriteAllText(folderPath + "/KilledEnemiesList.json", killedEnemiesSave);

        //Inventory
        InventorySave inventorySave = Inventory.instance.Save();
        string invSave = JsonUtility.ToJson(inventorySave, true);
        File.WriteAllText(folderPath + "/Inventory.json", invSave);
    }
}
