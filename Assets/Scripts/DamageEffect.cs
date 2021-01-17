using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public GameObject damageEffect;

    public void ShowDamageEffect()
    {
        foreach (var effect in damageEffect.GetComponentsInChildren<ParticleSystem>())
        {
            effect.Play();
        }
    }
}