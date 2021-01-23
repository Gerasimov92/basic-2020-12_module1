using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSettings : MonoBehaviour
{
    public Slider _slider;
    public AudioMixer _audioMixer;
    public string _groupName;

    private void Start()
    {
        _audioMixer.GetFloat(_groupName, out var value);
        _slider.value = value;
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(SliderValueChanged);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(SliderValueChanged);
    }

    private void SliderValueChanged(float value)
    {
        _audioMixer.SetFloat(_groupName, value);
    }
}