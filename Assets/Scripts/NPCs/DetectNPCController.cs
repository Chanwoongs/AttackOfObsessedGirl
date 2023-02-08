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

        Interact(player.transform);

        // start dialog
        StartCoroutine(DialogManager.Instance.ShowDialog(detectDialog, () =>
        {
            state = DetectNPCState.Dance;

            StartCoroutine(StartDance(player));
        }));

        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);
    }

    public void Interact(Transform initiator)
    {
        if (state == DetectNPCState.Idle)
        {
            state = DetectNPCState.Dialog;
            character.LookTowards(initiator.position);
        }
        // State == Dialog ��� �̹� ���� ���� ����
        else if (state == DetectNPCState.Dialog)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(finishedDialog));
        }
    }

    public IEnumerator StartDance(PlayerController player)
    {
        player.StartDance();
        character.Animator.IsDancing = true;

        yield return new WaitForSeconds(5.0f);
        StopDance(player);
    }

    public void StopDance(PlayerController player)
    {
        player.StopDance();
        character.Animator.IsDancing = false;
        StartCoroutine(DialogManager.Instance.ShowDialog(afterDanceDialog, () =>
        {
            state = DetectNPCState.Dialog;
            // ToDo : ���� ü�� ����
            // StartCoroutine();
        }));
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