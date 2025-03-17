using System.Collections.Generic;
using UnityEngine;

public class Poster : MonoBehaviour, IUsableObj
{
    [SerializeField] private Transform nearPoint;
    [SerializeField] private SlotItem poster;
    [SerializeField] private Material posterMat;
    [SerializeField] private Material shadowMat;
    [SerializeField] private bool active = false;
    [SerializeField] private int questID = 0;
    [SerializeField] private string subQuest;
    [SerializeField] private string newLogName;
    private PlayerController player;
    private MeshRenderer meshRenderer;
    private OutlineList outlines;
    private ObjectInfoLog objectInfoLog;

    private void Start()
    {
        active = false;
        if(QuestController.instance.HaveQuest(questID))
        {
            Quest quest = QuestController.instance.GetQuest(questID);
            if(quest.SubQuestIsComplete(subQuest))
                active = true;
        }
        player = FindFirstObjectByType<PlayerController>();
        meshRenderer = GetComponent<MeshRenderer>();
        outlines = GetComponent<OutlineList>();
        objectInfoLog = GetComponent<ObjectInfoLog>();
        if(outlines != null)
            outlines.Show(false);
        if (active)
        {
            meshRenderer.material = posterMat;
            objectInfoLog.SetNameObject(newLogName);
        }
        else
        {
            meshRenderer.material = shadowMat;
        }
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
        if(active)
            return;
        if(!Inventory.instance.PlayerHaveItem(poster))
        {
            return;
        }
        HideOutline();
        Inventory.instance.RemoveItem(poster);
        meshRenderer.material = posterMat;
        active = true;
        if (QuestController.instance.HaveQuest(questID))
        {
            Quest quest = QuestController.instance.GetQuest(questID);
            quest.SetCompleteSubQuest(subQuest);
        }
        objectInfoLog.SetNameObject(newLogName);
    }

    public void ShowOutline()
    {
        if(active)
            return;
        if (outlines == null)
            return;
        outlines.Show(true);
    }

    public void HideOutline()
    {
        if (active)
            return;
        if (outlines == null)
            return;
        outlines.Show(false);
    }
}
