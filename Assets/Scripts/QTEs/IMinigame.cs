using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IMinigame
{
    IEnumerator StartMinigame();
    event Action OnSuccess;
    IEnumerator DuringCoroutine { get; set; }
    Dialog DuringDialog { get; set; }
    Character LeftPerson { get; set; }
    Character RightPerson { get; set; }
    void SetInfo(Dialog dialog, Character leftPerson, Character rightPerson);

    bool IsSucceed { get; set; }
    bool IsPlaying { get; set; }
}