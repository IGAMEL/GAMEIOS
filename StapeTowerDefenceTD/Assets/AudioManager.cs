using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public Sound[] musics, sfxSounds;
    public AudioSource musicSource, source;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("music");
    }



    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musics, x => x.name == name);

        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        source.PlayOneShot(s.clip);
    }
}
