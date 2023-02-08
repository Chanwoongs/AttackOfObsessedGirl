using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum GameState
{ 
    FreeRoam, Battle, Dialog, Dance
}

public class GameController : MonoBehaviour
{
    public int MaxHP { get; private set; }
    public int PlayerHP { get; set; }

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    [SerializeField] HPBar hpBar;

    GameState state;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MaxHP = 100;
        PlayerHP = MaxHP;
        hpBar.SetHP((float)PlayerHP / MaxHP);

        playerController.OnStartedBattle += StartBattle;
        playerController.OnStartedDance += () => { state = GameState.Dance; };
        playerController.OnFinishedDance += () => {playerController.HandleUpdate(); };

        battleSystem.OnBattleOver += EndBattle;


        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if( state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    private void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        battleSystem.StartBattle();
    }

    private void EndBattle(bool hasWon)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    public void OnDetected(DetectNPCController npc)
    {
        StartCoroutine(npc.OnDetectPlayer(playerController));
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if(state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if(state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }

    public HPBar HpBar { get => hpBar; }
}
