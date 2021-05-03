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

    public void Play(string clipName, bool loop = false)
    {
        var dataSound = dataSounds.Find(sound => sound.name == clipName);
        if (dataSound == null)
            return;

        if (_audioSource.clip != dataSound.audioClip)
        {
            _audioSource.loop = loop;
            _audioSource.clip = dataSound.audioClip;
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

    [Serializable]
    private class DataSound
    {
        public string name;
        public AudioClip audioClip;
    }
}
