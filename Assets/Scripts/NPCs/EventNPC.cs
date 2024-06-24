using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EventNPCState
{
    Idle, Dialog, Event
}

public class EventNPC : MonoBehaviour, IInteractable, INPCEvent
{
    [SerializeField] Dialog beforeDialog;
    [SerializeField] Dialog duringDialog;
    [SerializeField] Dialog successDialog;
    [SerializeField] Dialog failDialog;
    [SerializeField] Dialog afterDialog;
    [SerializeField] GameObject minigame;
    [SerializeField] GameObject stalkerDistUI;

    EventNPCState state;
    Transform initiator;
    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public IEnumerator Interact(Transform initiator)
    {
        if (state == EventNPCState.Idle)
        {
            this.initiator = initiator;
            state = EventNPCState.Dialog;
            character.LookTowards(initiator.position);

            if (minigame != null && !minigame.GetComponent<IMinigame>().IsSucceed)
            {
                state = EventNPCState.Event;

                yield return ConversationManager.Instance.StartConversation(
                      beforeDialog,
                      initiator.GetComponent<Character>(),
                      GetComponent<Character>());

                minigame = Instantiate(minigame, transform);
                stalkerDistUI = Instantiate(stalkerDistUI, transform);

                minigame.GetComponent<IMinigame>().SetInfo(duringDialog, initiator.GetComponent<Character>(), GetComponent<Character>());

                yield return minigame.GetComponent<IMinigame>().StartMinigame();

                minigame.GetComponent<IMinigame>().OnSuccess += HandleOnSuccess;
                minigame.GetComponent<IMinigame>().OnSuccess += stalkerDistUI.GetComponent<INPCEvent>().HandleOnSuccess;
                stalkerDistUI.GetComponent<StalkerDistUI>().OnFailure += HandleOnFailure;
                stalkerDistUI.GetComponent<StalkerDistUI>().OnFailure += minigame.GetComponent<INPCEvent>().HandleOnFailure;
            }
            else
            {
                yield return ConversationManager.Instance.StartConversation(
                     afterDialog,
                     initiator.GetComponent<Character>(),
                     GetComponent<Character>());
            }

            state = EventNPCState.Idle;
        }
    }

    public void HandleOnSuccess() 
    {
        StartCoroutine(Succeed());
    }

    public void HandleOnFailure()
    {
        StartCoroutine(Failed());
    }

    public IEnumerator Succeed()
    {
        StopCoroutine(minigame.GetComponent<IMinigame>().DuringCoroutine);
        yield return ConversationManager.Instance.EndConversation();

        yield return ConversationManager.Instance.StartConversation(
            successDialog,
            initiator.GetComponent<Character>(),
            GetComponent<Character>(), null, null, 0.0f);
    }

    public IEnumerator Failed()
    {
        StopCoroutine(minigame.GetComponent<IMinigame>().DuringCoroutine);
        yield return ConversationManager.Instance.EndConversation();

        yield return ConversationManager.Instance.StartConversation(
            failDialog,
            GameController.Instance.Stalker.GetComponent<Character>(),
            initiator.GetComponent<Character>(), null, null, 0.0f);

        Destroy(stalkerDistUI);
        initiator.GetComponent<PlayerController>().StartBattle();
    }

    public void Update()
    {
        character.HandleUpdate();
    }
}


