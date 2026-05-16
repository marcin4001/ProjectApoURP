using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveInfo
{
    public int saveIndex;
    public int level = 0;
    public string location;
    public string date;
}

[System.Serializable]
public class SaveInfoList
{
    public SaveInfo[] saveInfos;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private SaveInfoList saveInfoList;
    private PlayerController playerController;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        saveInfoList = new SaveInfoList();
        string filePath = Application.persistentDataPath + "/SavesList.json";
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(data, saveInfoList);
        }
        else
        {
            saveInfoList.saveInfos = new SaveInfo[5];
            for(int i = 0; i < 5;  i++)
            {
                saveInfoList.saveInfos[i] = new SaveInfo();
                saveInfoList.saveInfos[i].saveIndex = i + 1;
                saveInfoList.saveInfos[i].level = -1;
                saveInfoList.saveInfos[i].location = "none";
                saveInfoList.saveInfos[i].date = "none";
            }
            string data = JsonUtility.ToJson(saveInfoList, true);
            File.WriteAllText(filePath, data);
        }
    }

    public void Save(int indexSave)
    {
        string folderPath = Application.persistentDataPath + "/Saves" + "/Save" + (indexSave + 1);
        
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
        SaveInfo saveInfo = new SaveInfo();
        saveInfo.saveIndex = indexSave + 1;
        saveInfo.level = GameParam.instance.level;
        saveInfo.location = GameParam.instance.location;
        saveInfo.date = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
        saveInfoList.saveInfos[indexSave] = saveInfo;
        string data = JsonUtility.ToJson(saveInfoList, true);
        File.WriteAllText(Application.persistentDataPath + "/SavesList.json", data);
        Debug.Log("The game has been saved");
    }

    public void Load(int indexSave)
    {
        string folderPath = Application.persistentDataPath + "/Saves" + "/Save" + (indexSave + 1);
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("Save file not found.");
            return;
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

    public string GetSaveInfo(int indexSave)
    {
        if (saveInfoList.saveInfos[indexSave].level < 0)
        {
            return "None";
        }
        string result = $"Save {saveInfoList.saveInfos[indexSave].saveIndex}\n";
        result += $"Level {saveInfoList.saveInfos[indexSave].level}\n";
        result += $"Date: {saveInfoList.saveInfos[indexSave].date}\n";
        result += $"Location: {saveInfoList.saveInfos[indexSave].location}";
        return result;
    }
}
