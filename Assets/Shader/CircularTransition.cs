using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CircularTransition : MonoBehaviour
{
    [SerializeField] 
    private Material ScreenTransitionMaterial;

    [SerializeField]
    private float transitionTime = 1f;

    [SerializeField]
    private string propertyName = "_Progress";

    [SerializeField]
    private Canvas transitionCanvas;

    public UnityEvent OnAscendingTransitionDone;
    public UnityEvent OnDescendingTransitionDone;

    public IEnumerator StartAscendingTransition()
    {
        float currentTime = 0;

        while (currentTime < transitionTime)
        {
            currentTime += Time.deltaTime;
            ScreenTransitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
            yield return null;
        }

        OnAscendingTransitionDone?.Invoke();
    }

    public IEnumerator StartDescendingTransition()
    {
        float currentTime = 0;

        while (currentTime < transitionTime)
        {
            currentTime += Time.deltaTime;
            ScreenTransitionMaterial.SetFloat(propertyName, Mathf.Clamp01((transitionTime - currentTime) / transitionTime));
            yield return null;
        }

        OnDescendingTransitionDone?.Invoke();
    }
}
