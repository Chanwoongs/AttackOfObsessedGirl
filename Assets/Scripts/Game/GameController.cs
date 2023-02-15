using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public enum GameState
{ 
    FreeRoam, Battle, Dialog, Dance, Stop
}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] GameObject transitions;
    [SerializeField] MGDoor mgDoor;
    [SerializeField] HPBar hpBar;
    [SerializeField] GameObject stalker;

    public GameState State { get; private set; }

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
        playerController.OnStartedDance += () => { State = GameState.Dance; };
        playerController.OnFinishedDance += () => {playerController.HandleUpdate(); };

        battleSystem.OnBattleOver += EndBattle;
        battleSystem.OnPerformSkill += UpdatePlayerItems;

        ConversationManager.Instance.OnShowDialog += () =>
        {
            State = GameState.Dialog;
        };
        ConversationManager.Instance.OnCloseDialog += () =>
        {
            if( State == GameState.Dialog)
                State = GameState.FreeRoam;
        };
    }

    public void StopUpdate()
    {
        State = GameState.Stop;
    }

    public void ResumeFreeRoamUpdate()
    {
        State = GameState.FreeRoam;
    }

    public void ResumeBattleUpdate()
    {
        State = GameState.Battle;
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

    public void MGChangeUI()
    {
        // UI 변경 효과 및 처리
        hpBar.ChangeHealthBar();
    }

    private void StartBattle()
    { 
        State = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        HpBar.gameObject.SetActive(false);
        battleSystem.StartBattle(playerController.Items);
    }

    private void EndBattle(bool hasWon)
    {
        State = GameState.FreeRoam;
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
        if (State == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if(State == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if(State == GameState.Dialog)
        {
            ConversationManager.Instance.HandleUpdate();
        }
    }

    public PlayerController PlayerController { get => playerController; }
    public GameObject Stalker { get => stalker; }
    public HPBar HpBar { get => hpBar; }
    public GameObject Transitions { get => transitions; }
}
