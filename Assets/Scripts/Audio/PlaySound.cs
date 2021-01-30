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

    public void Play(string nameClip, bool loop = false)
    {
        var audioClip = GetAudioClip(nameClip);
        if (_audioSource.clip != audioClip)
        {
            _audioSource.loop = loop;
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
        else if (!_audioSource.isPlaying)
        {
            _audioSource.loop = loop;
            _audioSource.Play();
        }
    }

    public void Stop()
    {
        _audioSource.Stop();
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
