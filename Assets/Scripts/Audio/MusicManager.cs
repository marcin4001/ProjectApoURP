using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private AudioSource source;
    [SerializeField] private List<AudioClip> themes = new List<AudioClip>();
    [SerializeField] private float maxVolume = 1f;

    private void Awake()
    {
        instance = this;
    }
    public void SetTheme(int index)
    {
        if(source == null || themes.Count == 0)
            return;
        source.clip = themes[index];
        source.Play();
        source.loop = true;
        source.volume = maxVolume;
    }

    public void SetMaxVolume(float volume)
    {
        maxVolume = volume;
        if(source != null)
            source.volume = maxVolume;
    }
}
