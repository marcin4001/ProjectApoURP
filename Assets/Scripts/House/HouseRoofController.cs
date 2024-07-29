using UnityEngine;

public class HouseRoof : MonoBehaviour
{
    [SerializeField] private GameObject roof;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            roof.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            roof.SetActive(true);
        }
    }
}
