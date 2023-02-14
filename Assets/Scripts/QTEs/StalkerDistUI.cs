using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StalkerDistUI : MonoBehaviour, INPCEvent
{
    [SerializeField] Image stalkerImage;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] float firstDistance;
    [SerializeField] float currentDistance;

    public event Action OnFailure; 

    public bool IsCaught { get; private set; }
    public bool IsEscaped { get; set; }

    private void OnEnable()
    {
        currentDistance = firstDistance;
    }

    public void HandleOnSuccess()
    {
        StartCoroutine(Succeed());
    }

    public void HandleOnFailure()
    {
        StartCoroutine(Failed());
    }

    public IEnumerator Succeed()
    {
        IsEscaped = true;
        Destroy(this.gameObject);

        yield return null;
    }

    public IEnumerator Failed()
    {
        OnFailure?.Invoke();

        yield return null;
    }

    private void Update()
    {
        if (IsCaught || IsEscaped) return;

        currentDistance -= Time.deltaTime*10;

        stalkerImage.transform.localScale = new Vector3(
            (firstDistance - currentDistance) / firstDistance * 2 + 1,
            (firstDistance - currentDistance) / firstDistance * 2 + 1);
        distanceText.text = "Stalker Distance : " + string.Format("{0:N1}", currentDistance) + "m";

        if (currentDistance <= 0f)
        {
            HandleOnFailure();
            IsCaught = true;
        }
    }
}
