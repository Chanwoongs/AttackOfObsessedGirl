using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState
{
    Idle, Dialog
}

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;

    NPCState state;
    float idleTimer = 0;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.LookTowards(initiator.position);

            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
            {
                idleTimer = 0f;
                state = NPCState.Idle;
            }));
        }
    }

    public void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
        }
        character.HandleUpdate();
    }
}
