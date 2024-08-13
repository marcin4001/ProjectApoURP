using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class InHouseTrigger : MonoBehaviour
{
    [SerializeField] private bool playerInTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger = false;
        }
    }

    public bool GetPlayerInTrigger()
    {
        return playerInTrigger;
    }
}
