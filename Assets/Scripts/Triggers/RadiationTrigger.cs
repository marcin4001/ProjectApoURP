using System.Collections;
using UnityEngine;

public class RadiationTrigger : MonoBehaviour
{
    [SerializeField] private float timeAddRadiation = 2f;
    private Coroutine coroutine;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(AddRadiation());
            source.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if(coroutine != null) 
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            source.Stop();
        }
    }

    private IEnumerator AddRadiation()
    {
        while(true)
        {
            yield return new WaitForSeconds(timeAddRadiation);
            if(PlayerStats.instance.RadLevelIsFull() && RadDeath.instance != null)
            {
                RadDeath.instance.SetDeath();
            }
            PlayerStats.instance.AddOneRadLevel();
        }
    }
}
