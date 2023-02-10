using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    public LayerMask NPCLayer;

    private bool isMoving;
    public bool IsDanceOver { get; private set; }

    public bool[] items;

    private Vector2 input;

    public event Action OnStartedBattle;
    public event Action OnStartedDance;
    public event Action OnFinishedDance;

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
        items = new bool[(int)BattleAction.Burger + 1];
    }

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            character.Animator.MoveX = input.x;
            character.Animator.MoveY = input.y;

            if (input != Vector2.zero)
            {
                character.Animator.IsMoving = true;

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
               
                OnMoveOver();
                CheckToStartBattle();
            }
            else
                character.Animator.IsMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Interact());
        }
    }

    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, GameLayers.i.TriggerableLayers);
        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
    }

    IEnumerator Interact()
    {
        var dir = character.Animator.FacingDir;
        var interactPos = transform.position + dir;
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            yield return collider.GetComponent<IInteractable>()?.Interact(transform);
        }
    }

    private void CheckToStartBattle()
    {
        // 배틀 시작 코드
        if (Physics2D.OverlapCircle(transform.position, 0.5f, NPCLayer))
        {
            character.Animator.IsMoving = false;
            OnStartedBattle();
        }
    }

    public IEnumerator DecreaseHP(int amount)
    {
        var targetHP = GameController.Instance.PlayerHP -= amount;

        yield return GameController.Instance.HpBar.SetHPSmooth((float)targetHP / GameController.Instance.MaxHP);
    }

    public void StopPlayer()
    {
        isMoving = false;
        character.Animator.IsMoving = false;
    }

    public void StartDance()
    {
        OnStartedDance();
        character.Animator.IsDancing = true;
    }

    public void StopDance()
    {
        OnFinishedDance();
        IsDanceOver = true;
        character.Animator.IsDancing = false;   
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public bool[] Items { get => items; }
}
