using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EffectGiver : MonoBehaviour
{
    [SerializeField] Dialog giveDialog;
    [SerializeField] Dialog buffDialog;
    [SerializeField] Dialog debuffDialog;
    [SerializeField] Dialog rejectDialog;

    bool isGiven = false;

    public IEnumerator GiveEffect(PlayerController player)
    {
        int selectedChoice = 0;

        yield return ConversationManager.Instance.StartConversation(
            giveDialog, GetComponent<Character>(), player.GetComponent<Character>(),
            new List<string>() { "Yes", "No" },
            (choiceIndex) => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            if (GameController.Instance.PlayerController.PlayerHP == 100)
            {
                GameController.Instance.PlayerController.PlayerHP -= 10;

                yield return ConversationManager.Instance.StartConversation(
                    debuffDialog, GetComponent<Character>(), player.GetComponent<Character>());
            }
            else
            {
                Mathf.Clamp(GameController.Instance.PlayerController.PlayerHP += 30, 0, 100);

                yield return ConversationManager.Instance.StartConversation(
                    buffDialog, GetComponent<Character>(), player.GetComponent<Character>());
            }

            isGiven = true;
        }
        else if (selectedChoice == 1)
        {
            // 아니요 처리
            yield return ConversationManager.Instance.StartConversation(
                rejectDialog, GetComponent<Character>(), player.GetComponent<Character>());
        }
    }

    public bool CanBeGiven()
    {
        return !isGiven;
    }
}
