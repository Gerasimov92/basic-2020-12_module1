using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static void SetCanvasGroupEnabled(CanvasGroup group, bool enabled)
    {
        AlphaAnimator alphaAnimator = group.GetComponent<AlphaAnimator>();
        if (alphaAnimator)
            alphaAnimator.targetAlpha = (enabled ? 1.0f : 0.0f);
        else
            group.alpha = (enabled ? 1.0f : 0.0f);
        
        group.interactable = enabled;
        group.blocksRaycasts = enabled;
    }
}
