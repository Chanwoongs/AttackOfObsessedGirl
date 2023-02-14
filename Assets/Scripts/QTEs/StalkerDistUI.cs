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
    [SerializeField] float changeDistance;
    [SerializeField] int changeCount;

    public event Action OnFailure; 

    public bool IsCaught { get; private set; }
    public bool IsEscaped { get; set; }

    private void OnEnable()
    {
        currentDistance = firstDistance;
        changeDistance = firstDistance / changeCount;
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

        currentDistance -= Time.deltaTime;
        
        if (firstDistance - currentDistance >= changeDistance)
        {
            stalkerImage.transform.localScale = new Vector3(
            stalkerImage.transform.localScale.x + 0.5f,
            stalkerImage.transform.localScale.y + 0.5f);

            changeDistance += firstDistance / changeCount;
        }
        distanceText.text = "Stalker Distance : " + string.Format("{0:N1}", currentDistance) + "m";

        if (currentDistance <= 0f)
        {
            HandleOnFailure();
            IsCaught = true;
        }
    }
}
