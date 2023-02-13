using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] BattleAction item;
    [SerializeField] Dialog giveDialog;
    [SerializeField] Dialog receiveDialog;
    [SerializeField] Dialog rejectDiaglog;

    bool isGiven = false;

    public IEnumerator GiveItem(PlayerController player)
    {
        int selectedChoice = 0;

        yield return ConversationManager.Instance.StartConversation(
            giveDialog, GetComponent<Character>(), player.GetComponent<Character>(),
            new List<string>() { "Yes", "No" },
            (choiceIndex) => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            player.Items.Add(item);
            
            yield return ConversationManager.Instance.StartConversation(
                receiveDialog, GetComponent<Character>(), player.GetComponent<Character>());

            isGiven = true;
        }
        else if (selectedChoice == 1)
        {
            yield return ConversationManager.Instance.StartConversation(
                rejectDiaglog, GetComponent<Character>(), player.GetComponent<Character>());
        }
    }

    public bool CanBeGiven()
    {
        return !isGiven;
    }
}
