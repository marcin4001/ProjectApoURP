using System.Collections;
using UnityEngine;

public class DialogueNPCTrigger : MonoBehaviour
{
    [SerializeField] private DialogueNPC npc;
    private PlayerController player;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
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
