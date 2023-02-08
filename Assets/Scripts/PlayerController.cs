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

            if (input != Vector2.zero)
            {
                character.Animator.IsMoving = true;

                character.Animator.MoveX = input.x;

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                CheckToStartBattle();
            }
            else
                character.Animator.IsMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    void Interact()
    {
        var dir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + dir;
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if (collider != null)
        {
            character.Animator.IsMoving = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        
    }


}
