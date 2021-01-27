using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UiElement : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Coroutine lastCoroutine;
    
    public void Show(bool smooth = false)
    {
        if(lastCoroutine != null)
            StopCoroutine(lastCoroutine);
        
        if (smooth)
            lastCoroutine = StartCoroutine(FadeTo(1.0f, 0.5f));
        else
            canvasGroup.alpha = 1.0f;
        
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    public void Hide(bool smooth = false)
    {
        if(lastCoroutine != null)
            StopCoroutine(lastCoroutine);
        
        if (smooth)
            lastCoroutine = StartCoroutine(FadeTo(0.0f, 0.5f));
        else
            canvasGroup.alpha = 0.0f;
        
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public bool IsVisible()
    {
        return canvasGroup.interactable;
    }
    
    IEnumerator FadeTo(float value, float time)
    {
        float alpha = canvasGroup.alpha;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            canvasGroup.alpha = Mathf.Lerp(alpha, value, t);
            yield return null;
        }

        canvasGroup.alpha = value;
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
