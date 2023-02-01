using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerAction,
    EnemyAction,
    Busy,

}


public class BattleSystem : MonoBehaviour
{
    [SerializeField] YeonwooBattle player;
    [SerializeField] EnemyBattle enemy;
    [SerializeField] PlayerHud playerHud;
    [SerializeField] EnemyHud enemyHud;

    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;
    
    private void Start()
    {
        StartCoroutine(SetUpBattle());
    }

    public IEnumerator SetUpBattle()
    {
        player.SetUp();
        enemy.SetUp();
        playerHud.SetData(player);
        enemyHud.SetData(enemy);

        yield return dialogBox.TypeDialog("Obsessed Girl started a battle!");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    private void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));

        dialogBox.EnableActionSelector(true);
    }

    private IEnumerator PerformPlayerAction()
    {
        state = BattleState.Busy;

        var action = player.GetCurrentActions()[currentAction].GetComponent<BattleActionComponent>();
        yield return dialogBox.TypeDialog(action.GetTriggerText());
        yield return new WaitForSeconds(1f);

        player.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemy.PlayHitAnimation();
        bool isLost = enemy.TakeAction(action);
        yield return dialogBox.TypeDialog(action.GetHitText());
        yield return new WaitForSeconds(1f);

        yield return enemyHud.UpdateHP();

        if (isLost)
        {
            enemy.PlayLoseAnimation();
            yield return dialogBox.TypeDialog("Obsessed Girl ran away!");
        }
        else
            StartCoroutine(EnemyAction());
    }

    private IEnumerator EnemyAction()
    {
        state = BattleState.EnemyAction;

        var action = enemy.GetAction().GetComponent<BattleActionComponent>();
        yield return dialogBox.TypeDialog(action.GetTriggerText());
        yield return new WaitForSeconds(1f);

        enemy.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        player.PlayHitAnimation();
        bool isLost = player.TakeAction(action);
        yield return playerHud.UpdateHP();

        if (isLost)
        {
            player.PlayLoseAnimation();
            yield return dialogBox.TypeDialog("I can't fight anymore....");
        }
        else
            PlayerAction();
    }

    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
    }

    private void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < player.GetCurrentActions().Count - 1)
                ++currentAction;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.SetActionTexts(player, player.GetCurrentActions().Count);
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerAction());
        }
    }
}
