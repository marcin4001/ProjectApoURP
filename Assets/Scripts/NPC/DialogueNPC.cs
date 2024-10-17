using UnityEngine;

public class DialogueNPC : MonoBehaviour, IUsableObj
{
    [SerializeField] private DialogueProfile profile;
    [SerializeField] private int indexNode = 0;
    [SerializeField] private Transform nearPoint;

    void Start()
    {
        string npcName = profile.npcName;
        NPCObjList.instance.AddNPC(npcName);
    }

    public bool CanUse()
    {
        return true;
    }

    public GameObject GetMainGameObject()
    {
        return gameObject;
    }

    public Vector3 GetNearPoint()
    {
        return nearPoint.position;
    }

    public void Use()
    {
        string npcName = profile.npcName;
        indexNode = NPCObjList.instance.GetIndexNode(npcName);
        bool init = NPCObjList.instance.isInit(npcName);
        DialogueController.instance.SetInit(init);
        DialogueController.instance.SetIndexNode(indexNode);
        DialogueController.instance.SetProfile(profile);
        DialogueController.instance.ShowFirstDialogue();
        NPCObjList.instance.SetInit(npcName);
    }
}
