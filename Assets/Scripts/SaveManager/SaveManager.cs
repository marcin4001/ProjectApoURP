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
        string cabinetSave = JsonUtility.ToJson(listCabinetSave, true);
        File.WriteAllText(folderPath + "/CabinetList.json", cabinetSave);

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
        Debug.Log("The game has been saved");
    }

    public void Load()
    {
        string folderPath = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("Save file not found.");
        }
        //GameParam
        string gameParamSave = File.ReadAllText(folderPath + "/GameParam.json");
        JsonUtility.FromJsonOverwrite(gameParamSave, GameParam.instance);

        //NPCList
        string npcSave = File.ReadAllText(folderPath + "/NPCList.json");
        NPCObjListSave nPCObjListSave = JsonUtility.FromJson<NPCObjListSave>(npcSave);
        NPCObjList.instance.Load(nPCObjListSave);

        //ListCabinet
        string cabinetSave = File.ReadAllText(folderPath + "/CabinetList.json");
        ListCabinetSave listCabinetSave = JsonUtility.FromJson<ListCabinetSave>(cabinetSave);
        ListCabinet.instance.Load(listCabinetSave);

        //PickUpObjList
        string pickUpObjSave = File.ReadAllText(folderPath + "/PickUpObjList.json");
        PickUpObjListSave pickUpObjListSave = JsonUtility.FromJson<PickUpObjListSave>(pickUpObjSave);
        PickUpObjList.instance.Load(pickUpObjListSave);

        //ListOffers
        string offersSave = File.ReadAllText(folderPath + "/ListOffers.json");
        ListOffersSave listOffersSave = JsonUtility.FromJson<ListOffersSave>(offersSave);
        ListOffers.instance.Load(listOffersSave);

        //QuestController
        string questsSave = File.ReadAllText(folderPath + "/QuestList.json");
        QuestListSave questListSave = JsonUtility.FromJson<QuestListSave>(questsSave);
        QuestController.instance.Load(questListSave);

        //KilledEnemiesList
        string killedEnemiesSave = File.ReadAllText(folderPath + "/KilledEnemiesList.json");
        KilledEnemiesListSave killedEnemiesListSave = JsonUtility.FromJson<KilledEnemiesListSave>(killedEnemiesSave);
        KilledEnemiesList.instance.Load(killedEnemiesListSave);

        //Inventory
        string invSave = File.ReadAllText(folderPath + "/Inventory.json");
        InventorySave inventorySave = JsonUtility.FromJson<InventorySave>(invSave);
        Inventory.instance.Load(inventorySave);
        GameParam.instance.loadSave = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameParam.instance.prevScene);
    }
}
