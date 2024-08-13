using UnityEngine;

public class HouseRoof : MonoBehaviour
{
    [SerializeField] private GameObject roof;
    [SerializeField] private Door[] doors;
    [SerializeField] private InHouseTrigger inHouseTrigger;
    [SerializeField] private bool playerInTrigger = false;
    private void Start()
    {
        doors = GetComponentsInChildren<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger = true;
            if(HouseIsOpen())
                roof.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInTrigger = false;
            roof.SetActive(true);
        }
    }

    public void Hide()
    {
        if(playerInTrigger)
        {
            roof.SetActive(false);
        }
    }

    public void Show()
    {
        if(!HouseIsOpen())
        {
            if(inHouseTrigger == null)
                roof.SetActive(true);
            if (!inHouseTrigger.GetPlayerInTrigger())
                roof.SetActive(true);
        }
    }

    private bool HouseIsOpen()
    {
        if(doors.Length == 0)
            return true;
        foreach(Door door in doors)
        {
            if(door.IsOpen())
                return true;
        }
        return false;
    }
}
