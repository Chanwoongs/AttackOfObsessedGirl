using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TabTabEvent : MonoBehaviour, IMinigame
{
    [SerializeField] Slider progressBar;
    public bool IsSucceed { get; set; }
    public bool IsPlaying { get; set; }

    public event Action OnSuccess;

    private void Start()
    {
        IsSucceed = false;
    }

    public IEnumerator StartMinigame()
    {
        progressBar.value = 0;
        IsPlaying = true;
        IsSucceed = false;

        yield return null;
    }

    public void HandleOnFailure()
    {
        StartCoroutine(Failed());
    }

    public IEnumerator Succeed()
    {
        // 성공 효과
        OnSuccess?.Invoke();
        IsPlaying = false;

        Destroy(this.gameObject);
        yield return null;
    }

    public IEnumerator Failed()
    {
        // 실패 효과
        IsPlaying = false;

        Destroy(this);
        yield return null;
    }

    private void Update()
    {
         if (IsSucceed) { return; }

        if (Input.GetKeyDown(KeyCode.Space) && IsPlaying)
        {
            progressBar.value += 50.0f;
        }

        if (progressBar.value > 99f)
        {
            IsSucceed = true;
            StartCoroutine(Succeed());
            return;
        }

        progressBar.value -= 1.0f * Time.deltaTime;
    }
}
