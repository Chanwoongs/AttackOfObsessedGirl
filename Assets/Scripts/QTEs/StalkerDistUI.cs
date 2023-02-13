using System;
using System.Collectdions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Collections;

public class StalkerDistUI : MonoBehaviour
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

    public void HandleOnSucceed()
    {
        StartCoroutine(Succeed());
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

        stalkerImage.transform.localScale = new Vector3(
            (firstDistance - currentDistance) / firstDistance * 2 + 1,
            (firstDistance - currentDistance) / firstDistance * 2 + 1);
        distanceText.text = "Stalker Distance : " + string.Format("{0:N1}", currentDistance) + "m";

        if (currentDistance <= 0f)
        {
            Failed();
            IsCaught = true;
        }
    }
}
