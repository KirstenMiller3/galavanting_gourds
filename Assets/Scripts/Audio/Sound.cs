using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public AudioClip clip;

    public string sName;

    public bool loop;

    public AudioMixerGroup mixerGroup;


    [HideInInspector]
    public AudioSource source;

}
