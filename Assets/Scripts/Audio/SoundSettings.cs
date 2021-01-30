using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class SoundSettings : MonoBehaviour
{
    [SerializeField]
    private List<MixerChannel> channels = new List<MixerChannel>();
    public AudioMixer audioMixer;

    private void Start()
    {
        foreach (var ch in channels)
        {
            ch.slider.onValueChanged.AddListener(SliderValueChanged);
            float value = PlayerPrefs.GetFloat(ch.name, 0);
            SetVolume(ch.name, value);
            ch.slider.value = value;
        }
    }

    private void SliderValueChanged(float value)
    {
        var sender = EventSystem.current.currentSelectedGameObject;
        foreach (var ch in channels)
        {
            if (ch.slider.gameObject == sender)
            {
                SetVolume(ch.name, value);
            }
        }
    }

    private void SetVolume(string name, float value)
    {
        audioMixer.SetFloat(name, value);
        PlayerPrefs.SetFloat(name, value);
    }

    [Serializable]
    private class MixerChannel
    {
        public string name;
        public Slider slider;
    }
}
