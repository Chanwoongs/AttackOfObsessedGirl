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

    private void Start()
    {
        dialog.SwitchingDatas.Add(new SwitchingData(1, SpriteState.Angry, SpriteState.Idle));
        dialog.SwitchingDatas.Add(new SwitchingData(2, SpriteState.Sad, SpriteState.Joy));
    }

    public void Interact(Transform initiator)
    {
        if (state == TalkNPCState.Idle)
        {
            state = TalkNPCState.Dialog;
            character.LookTowards(initiator.position);

            StartCoroutine(
                ConversationManager.Instance.StartConversation(
                    dialog,
                    initiator.GetComponent<Character>(),
                    GetComponent<Character>(), () =>
            {
                state = TalkNPCState.Idle;
            }));;
        }
    }

    public void Update()
    {
        character.HandleUpdate();
    }
}
