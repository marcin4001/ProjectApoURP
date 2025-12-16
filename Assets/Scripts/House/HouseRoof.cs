using UnityEngine;

public class HouseRoof : MonoBehaviour
{
    [SerializeField] private GameObject roof;
    [SerializeField] private Door[] doors;
    [SerializeField] private InHouseTrigger inHouseTrigger;
    [SerializeField] private bool playerInTrigger = false;
    [SerializeField] private bool openOnStart = false;
    [SerializeField] private bool withoutDoors = false;
    [SerializeField] private Light[] lights;
    private void Start()
    {
        if(lights == null)
            lights = new Light[0];
        if (lights.Length > 0)
        {
            foreach (Light light in lights)
            {
                if (light != null)
                    light.shadows = LightShadows.None;
            }
        }
        if (withoutDoors)
            return;
        doors = GetComponentsInChildren<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger = true;
            if (HouseIsOpen())
            {
                roof.SetActive(false);
                if (lights.Length > 0)
                {
                    foreach (Light light in lights)
                    {
                        if (light != null)
                            light.shadows = LightShadows.Soft;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInTrigger = false;
            roof.SetActive(true);

            if (lights.Length > 0)
            {
                foreach (Light light in lights)
                {
                    if (light != null)
                        light.shadows = LightShadows.None;
                }
            }
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
            if (inHouseTrigger == null)
            {
                roof.SetActive(true);
                return;
            }
            if (!inHouseTrigger.GetPlayerInTrigger())
                roof.SetActive(true);
        }
    }

    private bool HouseIsOpen()
    {
        if (openOnStart)
            return true;
        if (doors.Length == 0)
            return true;
        foreach(Door door in doors)
        {
            if(door.IsOpen())
                return true;
        }
        return false;
    }
}
