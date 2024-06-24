using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

    public event Action<bool> OnBattleOver;
    public event Action<BattleAction> OnPerformSkill;

    BattleState state;
    int currentAction;
    
    public void StartBattle(List<BattleAction> items)
    {
        StartCoroutine(SetUpBattle(items));
    }

    public IEnumerator SetUpBattle(List<BattleAction> items)
    {
        player.SetUp(items);
        enemy.SetUp();
        playerHud.SetData(player);
        enemyHud.SetData(enemy);

        yield return ShowBattleStartUI();

        yield return dialogBox.TypeDialog("Obsessed Girl started a battle!");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    private IEnumerator ShowBattleStartUI()
    {
        // 배틀 시작 시 시작 효과 UI
        yield return null;
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

        yield return dialogBox.TypeDialog(action.TriggerText);
        yield return new WaitForSeconds(1f);

        player.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemy.PlayHitAnimation();
        bool isEnemyLost = enemy.TakeAction(action);
        yield return dialogBox.TypeDialog(action.HitText);
        yield return new WaitForSeconds(1f);

        yield return enemyHud.UpdateHP();

        if (action.Type != BattleActionType.Attack)
        {
            OnPerformSkill(action.Action);
            player.GetCurrentActions().RemoveAt(currentAction);
        }

        currentAction = 0;

        if (isEnemyLost)
        {
            // HP가 10 이하일 때 마지막 결투에 대한 처리 해주기
            enemy.PlayLoseAnimation();
            yield return dialogBox.TypeDialog("Obsessed Girl ran away!");

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
            StartCoroutine(EnemyAction());
    }

    private IEnumerator EnemyAction()
    {
        state = BattleState.EnemyAction;

        var action = enemy.GetAction().GetComponent<BattleActionComponent>();
        yield return dialogBox.TypeDialog(action.TriggerText);
        yield return new WaitForSeconds(1f);

        enemy.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        player.PlayHitAnimation();
        bool isPlayerLost = player.TakeAction(action);
        yield return playerHud.UpdateHP();

        if (isPlayerLost)
        {
            player.PlayLoseAnimation();
            yield return dialogBox.TypeDialog("I can't fight anymore....");

            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else
            PlayerAction();
    }

    public void HandleUpdate()
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

        if (Input.GetKeyDown(KeyCode.Space) && !dialogBox.IsTyping)
        {
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerAction());
        }
    }
}
