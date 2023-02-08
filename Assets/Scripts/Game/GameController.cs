using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{ 
    FreeRoam, Battle, Dialog, Dance
}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerController.OnStartedBattle += StartBattle;
        playerController.OnStartedDance += () => { state = GameState.Dance; };
        playerController.OnFinishedDance += () => {playerController.HandleUpdate(); };
        playerController.OnDetected += (Collider2D trainerCollider) =>
        {
            var detectNPC = trainerCollider.GetComponentInParent<DetectNPCController>();
            if (detectNPC != null)
            {
                StartCoroutine(detectNPC.OnDetectPlayer(playerController));
            }
        };

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
}
