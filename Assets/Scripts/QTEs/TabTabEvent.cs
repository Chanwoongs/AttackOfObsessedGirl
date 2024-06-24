using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TabTabEvent : MonoBehaviour, IMinigame, INPCEvent
{
    [SerializeField] Slider progressBar;
    public bool IsSucceed { get; set; }
    public bool IsPlaying { get; set; }
    public Dialog DuringDialog { get; set; }
    public Character LeftPerson { get; set; }
    public Character RightPerson { get; set; }
    public IEnumerator DuringCoroutine { get; set; }

    public event Action OnSuccess;

    private void Start()
    {
        IsSucceed = false;
    }

    public void SetInfo(Dialog dialog, Character leftPerson, Character rightPerson)
    {
        DuringDialog = dialog;
        LeftPerson = leftPerson;
        RightPerson = rightPerson;
    }

    public IEnumerator StartMinigame()
    {
        progressBar.value = 0;
        IsPlaying = true;
        IsSucceed = false;

        DuringCoroutine = AutoTalk();
        StartCoroutine(DuringCoroutine);

        yield return null;
    }

    public IEnumerator AutoTalk()
    {
        yield return (
        ConversationManager.Instance.StartConversation(
            DuringDialog,
            LeftPerson,
            RightPerson, null, null, 4.0f));
    }

    public void HandleOnFailure()
    {
        StartCoroutine(Failed());
    }

    public void HandleOnSuccess()
    {
        StartCoroutine(Succeed());
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

        Destroy(this.gameObject);
        yield return null;
    }

    private void Update()
    {
         if (IsSucceed) { return; }

        if (Input.GetKeyDown(KeyCode.Space) && IsPlaying)
        {
            progressBar.value += 3.0f;
        }

        if (progressBar.value > 99.5f)
        {
            IsSucceed = true;
            HandleOnSuccess();
            return;
        }

        progressBar.value -= Time.deltaTime * 3f;
    }
}
