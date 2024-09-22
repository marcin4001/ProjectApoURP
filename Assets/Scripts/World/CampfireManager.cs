using UnityEngine;

public class CampfireManager : MonoBehaviour
{
    [SerializeField] private Campfire[] campfires;

    public void SwitchOn()
    {
        foreach (Campfire camp in campfires)
        {
            if (camp != null)
                camp.Show(true);
        }
    }

    public void SwitchOff()
    {
        foreach (Campfire camp in campfires)
        {
            if(camp != null)
                camp.Show(false);
        }
    }
}
