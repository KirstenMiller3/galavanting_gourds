using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixerGroup;


        }




        Play("main");

    }

    public void Play(string name)
    {
       Sound s = Array.Find(sounds, sound => sound.sName == name);

        if(s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogError($"Sound {name} was not found.");
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.sName == name);

        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogError($"Sound {name} was not found.");
        }
    }

    public AudioMixer masterMixer;

    public void SetSefxLevel(float level)
    {
        masterMixer.SetFloat("sfxVol", level);
    }

    public void SetMusicLevel(float level)
    {
        masterMixer.SetFloat("musicVol", level);
    }
}