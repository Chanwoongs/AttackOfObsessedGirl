using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor;

    [SerializeField] TextMeshProUGUI dialogText;

    [SerializeField] GameObject actionSelector;

    [SerializeField] List<TextMeshProUGUI> actionTexts;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i == selectedAction)
                actionTexts[i].faceColor = highlightedColor;
            else
                actionTexts[i].faceColor = Color.black;
        }
    }

    public void SetActionTexts(YeonwooBattle yeonwoo, int currentActionCount)
    {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i < currentActionCount)
            {
                actionTexts[i].text = yeonwoo.GetCurrentActions()[i].name;
            }
            else
                actionTexts[i].text = "";
        }
    }
}
