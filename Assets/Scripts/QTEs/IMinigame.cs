using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IMinigame
{
    IEnumerator StartMinigame();
    IEnumerator Succeed();
    IEnumerator Failed();

    event Action OnSuccess;
    event Action OnFailure;

    bool IsSucceed { get; set; }
    bool IsPlaying { get; set; }
}