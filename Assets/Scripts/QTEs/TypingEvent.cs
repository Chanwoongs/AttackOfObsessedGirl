using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ArrowType
{
    Up, Down, Left, Right, Max
}

public class TypingKeyData
{
    public TypingKeyData(Sprite sprite, ArrowType arrowType, KeyCode arrowKey) 
    {
        Sprite = sprite; ArrowType = arrowType; ArrowKey = arrowKey;
    }
    public Sprite Sprite { get; set; }
    public ArrowType ArrowType { get; set; }    
    public KeyCode ArrowKey { get; set; } 
}

public class TypingEvent : MonoBehaviour, IMinigame, INPCEvent
{
    [SerializeField] Slider ProgressBar;

    [SerializeField] Transform keys;
    [SerializeField] Sprite up;
    [SerializeField] Sprite down;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;
    [SerializeField] Sprite empty;

    private Sprite sprite;

    private LinkedList<TypingKeyData> typingKeyDatas = new LinkedList<TypingKeyData>();
    private TypingKeyData ActiveKey;

    public bool IsSucceed { get; set; }
    public bool IsPlaying { get; set; }
    public IEnumerator DuringCoroutine { get; set; }
    public Dialog DuringDialog { get; set; }
    public Character LeftPerson { get; set; }
    public Character RightPerson { get; set; }

    public event Action OnClearLine;
    public event Action OnSuccess;

    public void SetInfo(Dialog dialog, Character leftPerson, Character rightPerson)
    {
        DuringDialog = dialog;
        LeftPerson = leftPerson;
        RightPerson = rightPerson;
    }

    public IEnumerator StartMinigame()
    {
        Initialize();
        IsPlaying = true;

        OnClearLine += SetUpNextLine;

        DuringCoroutine = DuringTalk();
        StartCoroutine(DuringCoroutine);

        yield return null;
    }

    public IEnumerator DuringTalk()
    {
        yield return (
            ConversationManager.Instance.StartConversation(
                DuringDialog,
                LeftPerson,
                RightPerson, null, null, 0.0f, true));
    }
    private void Initialize()
    {
        for (int i = 0; i < 7; i++)
        {
            var keyData = InitializeKey();
            typingKeyDatas.AddFirst(keyData);
        }
    }
    
    private void ResetKeys()
    {
        foreach (var data in typingKeyDatas)
        {
            var keyData = InitializeKey();
            data.Sprite = keyData.Sprite;
            data.ArrowType = keyData.ArrowType;
            data.ArrowKey = keyData.ArrowKey;
        }
    }

    private TypingKeyData InitializeKey()
    {
        KeyCode arrowKey = new KeyCode();

        var arrowType = (ArrowType)UnityEngine.Random.Range(0, (int)ArrowType.Max);

        switch (arrowType)
        {
            case ArrowType.Up:
                arrowKey = KeyCode.UpArrow;
                sprite = up;
                break;
            case ArrowType.Down:
                arrowKey = KeyCode.DownArrow;
                sprite = down;
                break;
            case ArrowType.Left:
                arrowKey = KeyCode.LeftArrow;
                sprite = left;
                break;
            case ArrowType.Right:
                arrowKey = KeyCode.RightArrow;
                sprite = right;
                break;
        }

        return new TypingKeyData(sprite, arrowType, arrowKey);
    }

    private void SwitchKeys()
    {
        var temp = typingKeyDatas.First;
        typingKeyDatas.RemoveFirst();
        EmptyKey(temp.Value);
        typingKeyDatas.AddLast(temp);
    }

    private void EmptyKey(TypingKeyData data)
    {
        data.Sprite = empty;
        data.ArrowKey = KeyCode.None;
        data.ArrowType = ArrowType.Max;
    }

    private void SetUpNextLine()
    {
        ProgressBar.value += 20;
        ConversationManager.Instance.ShowNext = true;
        ResetKeys();
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

    public void HandleOnSuccess()
    {
        StartCoroutine(Succeed());
    }

    public void HandleOnFailure()
    {
        StartCoroutine(Failed());
    }

    private void Update()
    {
        if (!IsPlaying || IsSucceed) { return; }

        UpdateKeys();

        if (Input.GetKeyDown(ActiveKey.ArrowKey))
        {
            // 맞은 효과 처리
            // StartCoroutine(Effect);
            SwitchKeys();
        }
        if (ProgressBar.value > 99.5f)
        {
            IsSucceed = true;
            HandleOnSuccess();
            return;
        }
    }

    private void UpdateKeys()
    {
        if (typingKeyDatas.First.Value.ArrowType == ArrowType.Max)
        {
            OnClearLine?.Invoke();
            return;
        }

        int i = 0;
        ActiveKey = typingKeyDatas.First.Value;

        foreach (var data in typingKeyDatas)
        {
            keys.GetChild(i).GetComponent<Image>().sprite = data.Sprite;
            i++;
        }
    }
}
