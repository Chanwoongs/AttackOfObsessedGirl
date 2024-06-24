using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectNPCState
{
    Idle, Dialog, Dance
}

public class DetectNPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog detectDialog;
    [SerializeField] Dialog afterDanceDialog;
    [SerializeField] Dialog finishedDialog;
    [SerializeField] GameObject exclamation;

    public bool HasDetected = false;

    DetectNPCState state;
    float idleTimer = 0;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public IEnumerator OnDetectPlayer(PlayerController player)
    {
        HasDetected = true;

        // show exclamation
        exclamation.SetActive(true);
        player.StopPlayer();

        state = DetectNPCState.Dance;
        Interact(player.transform);

        // start dialog
        yield return ConversationManager.Instance.StartConversation(detectDialog, null, null);
        StartCoroutine(StartDance(player));

        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (state == DetectNPCState.Dance)
        {
            state = DetectNPCState.Dialog;
            character.LookTowards(initiator.position);
        }
        // State == Dialog 라면 이미 춤이 끝난 상태
        else if (state == DetectNPCState.Dialog)
        {
            yield return ConversationManager.Instance.StartConversation(finishedDialog);
        }
    }

    public IEnumerator StartDance(PlayerController player)
    {
        player.StartDance();
        character.Animator.IsDancing = true;

        yield return new WaitForSeconds(5.0f);
        StartCoroutine(StopDance(player));
    }

    public IEnumerator StopDance(PlayerController player)
    {
        player.StopDance();
        character.Animator.IsDancing = false;
        yield return ConversationManager.Instance.StartConversation(afterDanceDialog, null, null);
        
        state = DetectNPCState.Dialog;
        // ToDo : 연우 체력 감소
        player.PlayerHP -= 10;
    }

    public void Update()
    {
        if (state == DetectNPCState.Idle)
        {
            idleTimer += Time.deltaTime;
        }
        character.HandleUpdate();
    }
}