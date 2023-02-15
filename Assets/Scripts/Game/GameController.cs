using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public enum GameState
{ 
    FreeRoam, Battle, Dialog, Dance, MG
}

public class GameController : MonoBehaviour
{
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
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        hpBar.SetHP((float)playerController.PlayerHP / playerController.MaxHP);

        playerController.OnStartedBattle += StartBattle;
        playerController.OnStartedDance += () => { state = GameState.Dance; };
        playerController.OnFinishedDance += () => {playerController.HandleUpdate(); };

        battleSystem.OnBattleOver += EndBattle;
        battleSystem.OnPerformSkill += UpdatePlayerItems;

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

    private void UpdatePlayerItems(BattleAction action)
    {
        foreach (var item in playerController.Items)
        {
            if (action == item)
            {
                playerController.Items.Remove(item);
                break;
            }
        }
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
        HpBar.gameObject.SetActive(false);
        battleSystem.StartBattle(playerController.Items);
    }

    private void EndBattle(bool hasWon)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        HpBar.gameObject.SetActive(true);
        hpBar.SetHP((float)playerController.PlayerHP / playerController.MaxHP);
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

    public PlayerController PlayerController { get => playerController; }
    public HPBar HpBar { get => hpBar; }
    public GameObject Transitions { get => transitions; }
}
