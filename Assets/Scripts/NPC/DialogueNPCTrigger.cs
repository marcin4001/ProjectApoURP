using System.Collections;
using UnityEngine;

public class DialogueNPCTrigger : MonoBehaviour
{
    [SerializeField] private DialogueNPC npc;
    [SerializeField] private bool checkIsInitNpc = false;
    [SerializeField] private string npcName;
    private PlayerController player;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (checkIsInitNpc)
        {
            Debug.Log("NPCObjList.instance.isInit(npcName): " + NPCObjList.instance.isInit(npcName));
            if(NPCObjList.instance.isInit(npcName))
                return;
        }
        if(other.tag == "Player")
        {
            StartCoroutine(StartDialogue());
        }
    }

    private IEnumerator StartDialogue()
    {
        player.StopMove();
        yield return new WaitForSeconds(0.05f);
        GameObject usableObj = npc.GetMainGameObject();
        player.transform.rotation = Quaternion.LookRotation(usableObj.transform.position - player.transform.position);
        npc.Use();
        Destroy(gameObject); 
    }
}
