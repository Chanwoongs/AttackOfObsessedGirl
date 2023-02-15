using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerTrigger : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] Dialog dialog;
    bool isVisited;

    public void OnPlayerTriggered(PlayerController player)
    {
        if (isVisited) return;

        isVisited = true;
        player.StopPlayer();
        StartCoroutine(StalkerComeAndStartBattle(player));
    }

    public IEnumerator StalkerComeAndStartBattle(PlayerController player)
    {
        yield return ConversationManager.Instance.StartConversation(dialog, null, GameController.Instance.Stalker.GetComponent<Character>());

        GameController.Instance.StopUpdate();

        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartDescendingTransition();

        player.StartBattle();

        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartAscendingTransition();

        GameController.Instance.ResumeBattleUpdate();
    }
}
