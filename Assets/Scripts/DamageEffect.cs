using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public GameObject damageEffect;

    private ParticleSystem[] effects;

    void Start()
    {
        effects = damageEffect.GetComponentsInChildren<ParticleSystem>();
    }

    public void ShowDamageEffect()
    {
        foreach (var effect in effects)
        {
            effect.Play();
        }
    }
}
