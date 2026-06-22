using System.Collections;
using UnityEngine;

public class DialogueNPCTrigger : MonoBehaviour
{
    [SerializeField] private DialogueNPC npc;
    [SerializeField] private bool checkIsInitNpc = false;
    [SerializeField] private string npcName;
    [SerializeField] private bool teleport = false;
    [SerializeField] private Transform teleportPos;
    [SerializeField] private bool rotateNPC;
    private PlayerController player;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(npc == null)
        {
            Destroy(gameObject);
            return;
        }
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
        if(teleport && !PlayerInFovNPC())
        {
            player.GetAgent().Warp(teleportPos.position);
            CameraMovement.instance.CenterCameraToPlayer();
        }
        player.StopMove();
        yield return new WaitForSeconds(0.05f);
        if (npc != null)
        {
            GameObject usableObj = npc.GetMainGameObject();
            player.transform.rotation = Quaternion.LookRotation(usableObj.transform.position - player.transform.position);
            if (rotateNPC)
                npc.transform.rotation = Quaternion.LookRotation(player.transform.position - usableObj.transform.position);
            npc.Use();
        }
        Destroy(gameObject); 
    }

    public bool PlayerInFovNPC()
    {
        Vector3 dirToPlayer = (player.transform.position - npc.transform.position).normalized;
        float angle = Vector3.Angle(npc.transform.forward, dirToPlayer);
        return angle < 60f;
    }
}
