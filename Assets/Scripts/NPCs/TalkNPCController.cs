using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TalkNPCState
{
    Idle, Dialog
}

public class TalkNPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;

    TalkNPCState state;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initiator)
    {
        if (state == TalkNPCState.Idle)
        {
            state = TalkNPCState.Dialog;
            character.LookTowards(initiator.position);

            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
            {
                state = TalkNPCState.Idle;
            }));
        }
    }

    public void Update()
    {
        character.HandleUpdate();
    }
}
