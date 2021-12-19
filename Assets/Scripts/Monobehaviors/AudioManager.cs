using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioEventSO musicEvent;
    private AudioSource musicAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
        musicAudioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        musicEvent.Raise();
    }
    public void StopMusic()
    {
        var musicObj = GameObject.Find("Music");
        Destroy(musicObj);
    }
}
