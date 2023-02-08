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
    private Vector2 input;

    public event Action OnStartedBattle;
    public event Action<Collider2D> OnDetected;

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();  
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
            }
            else
                character.Animator.IsMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    private void OnMoveOver()
    {
        CheckToStartBattle();
        CheckDetectionByNPC();
    }

    void Interact()
    {
        var dir = character.Animator.FacingDir;
        var interactPos = transform.position + dir;
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<IInteractable>()?.Interact(transform);
        }
    }

    private void CheckToStartBattle()
    {
        // 배틀 시작 코드
        //if (Physics2D.OverlapBox(transform.position, new Vector2(0.1f, 0.1f), NPCLayer))
        //{
        //    character.Animator.IsMoving = false;
        //    OnStartedBattle();
        //}
    }

    private void CheckDetectionByNPC()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.DetectLayer);
        if (collider != null && 
            Mathf.Abs(collider.GetComponentInParent<Transform>().position.x - transform.position.x) < 0.1f && 
            !collider.GetComponentInParent<DetectNPCController>().HasDetected)
        {
            OnDetected?.Invoke(collider);
        }
    }

    public void StopPlayer()
    {
        isMoving = false;
        character.Animator.IsMoving = false;
    }
}
