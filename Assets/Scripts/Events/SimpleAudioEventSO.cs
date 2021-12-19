using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SimpleAudioEventSO", menuName = "chicken_revolution/Audio/SimpleAudioEventSO", order = 0)]
public class SimpleAudioEventSO : AudioEventSO
{
    public Sound[] sounds;
    public override void Play()
    {
        if(sounds.Length == 0) return;

        // if(source == null)
        // {
        var randomNumber = Random.Range(0,sounds.Length);
        var soundObjectName = sounds[randomNumber].loop ? "Music":"SFX" ;
        var obj = new GameObject(name: soundObjectName, typeof(AudioSource));
        // obj.AddComponent<AudioSource>();
        var source = obj.GetComponent<AudioSource>();
        // }

        source.clip = sounds[randomNumber].clip;
        source.priority = sounds[randomNumber].priority;
        source.volume = sounds[randomNumber].volume;
        source.pitch = sounds[randomNumber].pitch;
        source.loop = sounds[randomNumber].loop;
        source.Play();
        if(!source.loop)
        {
            Destroy(source.gameObject, source.clip.length/source.pitch);
        }
    }
}