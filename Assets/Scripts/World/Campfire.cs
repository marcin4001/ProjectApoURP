using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] private GameObject fireFx;
    [SerializeField] private AudioSource source;

    public void Show(bool active)
    {
        fireFx.SetActive(active);
        if(source != null)
        {
            if (active)
                source.Play();
            else
                source.Stop();
        }
    }
}
