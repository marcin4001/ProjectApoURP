using UnityEngine;

public class DialogueNPC : MonoBehaviour, IUsableObj
{
    [SerializeField] private DialogueProfile profile;
    [SerializeField] private int indexNode = 0;
    [SerializeField] private Transform nearPoint;
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
        DialogueController.instance.SetIndexNode(indexNode);
        DialogueController.instance.SetProfile(profile);
        DialogueController.instance.ShowFirstDialogue();
    }
}
