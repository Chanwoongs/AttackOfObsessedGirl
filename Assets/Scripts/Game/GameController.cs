using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;


public enum GameState
{ 
    FreeRoam, Battle, Dialog, Dance, MG
}

public class GameController : MonoBehaviour
{
    public int MaxHP { get; private set; }
    public int PlayerHP { get; set; }

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] GameObject transitions;
    [SerializeField] MGDoor mgDoor;

    [SerializeField] HPBar hpBar;

    private GameState state;

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

        mgDoor.OnPlayerVisit += () =>
        {
            state = GameState.MG;
        };
        mgDoor.OnPlayerExit += () => 
        { 
            state = GameState.FreeRoam;
        };
        mgDoor.OnChangeUI += MGChangeUI;

        ConversationManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        ConversationManager.Instance.OnCloseDialog += () =>
        {
            if( state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    private void MGChangeUI()
    {
        // UI 변경 효과 및 처리
        hpBar.ChangeHealthBar();
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
            ConversationManager.Instance.HandleUpdate();
        }
    }

    public HPBar HpBar { get => hpBar; }
    public GameObject Transitions { get => transitions; }
}
