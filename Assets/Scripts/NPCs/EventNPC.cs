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
    IEnumerator autoTalk;

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
                      GetComponent<Character>(), null, null, 0.0f);

                minigame = Instantiate(minigame, transform);
                stalkerDistUI = Instantiate(stalkerDistUI, transform);

                yield return minigame.GetComponent<IMinigame>().StartMinigame();

                minigame.GetComponent<IMinigame>().OnSuccess += HandleOnSuccess;
                minigame.GetComponent<IMinigame>().OnSuccess += stalkerDistUI.GetComponent<StalkerDistUI>().HandleOnSuccess;
                stalkerDistUI.GetComponent<StalkerDistUI>().OnFailure += HandleOnFailure;
                stalkerDistUI.GetComponent<StalkerDistUI>().OnFailure += minigame.GetComponent<IMinigame>().HandleOnFailure;

                autoTalk = ConversationManager.Instance.StartConversation(
                    duringDialog,
                    initiator.GetComponent<Character>(),
                    GetComponent<Character>(), null, null, 4.0f);
                StartCoroutine(autoTalk);
            }
            else
            {
                yield return ConversationManager.Instance.StartConversation(
                     afterDialog,
                     initiator.GetComponent<Character>(),
                     GetComponent<Character>(), null, null, 0.0f);
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
        StopCoroutine(autoTalk);
        yield return ConversationManager.Instance.EndConversation();

        yield return ConversationManager.Instance.StartConversation(
            successDialog,
            initiator.GetComponent<Character>(),
            GetComponent<Character>(), null, null, 0.0f);
    }

    public IEnumerator Failed()
    {
        StopCoroutine(autoTalk);
        yield return ConversationManager.Instance.EndConversation();

        yield return ConversationManager.Instance.StartConversation(
            failDialog,
            initiator.GetComponent<Character>(),
            GetComponent<Character>(), null, null, 0.0f);

        Destroy(stalkerDistUI);
        initiator.GetComponent<PlayerController>().StartBattle();
    }

    public void Update()
    {
        character.HandleUpdate();
    }
}


