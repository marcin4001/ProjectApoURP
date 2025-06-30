using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private AudioSource source;
    [SerializeField] private List<AudioClip> themes = new List<AudioClip>();
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private AudioSource[] sfxSources;

    private void Awake()
    {
        instance = this;
        sfxSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        SetVolumeMainMusic(GameParam.instance.mainMusicVolume);
        SetVolumeSFX(GameParam.instance.sfxVolume);
    }

    private void Start()
    {
        
    }

    public void SetTheme(int index)
    {
        if(source == null || themes.Count == 0)
            return;
        source.clip = themes[index];
        source.Play();
        source.loop = true;
        //source.volume = maxVolume;
    }

    public void SetMaxVolume(float volume)
    {
        maxVolume = volume;
        SetVolumeMainMusic(GameParam.instance.mainMusicVolume);
    }

    public void SetVolumeMainMusic(float volume)
    {
        source.volume = volume * maxVolume;
    }

    public void SetVolumeSFX(float volume)
    {
        foreach(AudioSource sourceSfx in sfxSources)
        {
            if(sourceSfx == source)
                continue;
            sourceSfx.volume = volume;
        }
    }
}
