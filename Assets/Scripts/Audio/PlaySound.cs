using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private List<DataSound> dataSounds = new List<DataSound>();

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(string nameClip)
    {
        var audioClip = GetAudioClip(nameClip);
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    private AudioClip GetAudioClip(string nameClip)
    {
        AudioClip clip = null;
        
        foreach (var sound in dataSounds)
        {
            if (sound.name == nameClip)
                clip = sound.audioClip;
        }

        return clip;
    }
    
    [Serializable]
    private class DataSound
    {
        public string name;
        public AudioClip audioClip;
    }
}