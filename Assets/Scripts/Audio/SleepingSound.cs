using UnityEngine;

public class SleepingSound : MonoBehaviour
{
    public static SleepingSound instance;
    private AudioSource source;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySleepingClip()
    {
        source.Play();
    }
}
