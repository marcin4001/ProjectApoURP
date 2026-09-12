using UnityEngine;

public class NPCHatController : MonoBehaviour
{
    [SerializeField] private GameObject hat;
    [SerializeField] private int idQuest;
    void Start()
    {
        if(!QuestController.instance.Complete(idQuest))
            hat.SetActive(false);
    }

    public void ShowHat()
    {
        hat.SetActive(true);
    }
}
