using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] BattleAction item;
    [SerializeField] Dialog giveDialog;
    [SerializeField] Dialog receiveDialog;

    bool isGiven = false;

    public IEnumerator GiveItem(PlayerController player)
    {
        yield return ConversationManager.Instance.StartConversation(
            giveDialog, GetComponent<Character>(), player.GetComponent<Character>());

        player.Items[(int)item] = true;

        yield return ConversationManager.Instance.StartConversation(
            receiveDialog, GetComponent<Character>(), player.GetComponent<Character>());

        isGiven = true;
    }

    public bool CanBeGiven()
    {
        return !isGiven;
    }
}
