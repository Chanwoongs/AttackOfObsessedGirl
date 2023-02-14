using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TalkNPCState
{
    Idle, Dialog
}

public class TalkNPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;

    TalkNPCState state;

    Character character;
    ItemGiver itemGiver;
    EffectGiver effectGiver;

    private void Awake()
    {
        character = GetComponent<Character>();
        itemGiver = GetComponent<ItemGiver>();
        effectGiver = GetComponent<EffectGiver>();  
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (state == TalkNPCState.Idle)
        {
            state = TalkNPCState.Dialog;
            character.LookTowards(initiator.position);

            yield return
              ConversationManager.Instance.StartConversation(
                  dialog,
                  initiator.GetComponent<Character>(),
                  GetComponent<Character>(), null, null, 0.0f);

            if (itemGiver != null && itemGiver.CanBeGiven())
                yield return itemGiver.GiveItem(initiator.GetComponent<PlayerController>());
            if (effectGiver != null && effectGiver.CanBeGiven())
                yield return effectGiver.GiveEffect(initiator.GetComponent<PlayerController>());
          
            state = TalkNPCState.Idle;
        }
    }

    public void Update()
    {
        character.HandleUpdate();
    }
}
