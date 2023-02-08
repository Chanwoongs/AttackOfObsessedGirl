using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectNPCState
{
    Idle, Dialog
}

public class DetectNPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;
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
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
        {
            state = DetectNPCState.Idle;
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
