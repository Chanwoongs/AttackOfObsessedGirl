using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    public LayerMask NPCLayer;

    private bool isMoving;
    private Vector2 input;

    private CharacterAnimator animator;

    public event Action OnStartedBattle;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                animator.IsMoving = true;

                animator.MoveX = input.x;

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                CheckToStartBattle();
            }
            else
                animator.IsMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    void Interact()
    {
        var dir = new Vector3(animator.MoveX, animator.MoveY);
        var interactPos = transform.position + dir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<IInteractable>()?.Interact();
        }
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void CheckToStartBattle()
    {
        //if (Physics2D.OverlapCircle(transform.position, 0.1f, NPCLayer) != null)
        //{
        //    animator.IsMoving = false;
        //    OnStartedBattle();
        //}
        if (Physics2D.OverlapBox(transform.position, new Vector2(0.1f, 0.1f), NPCLayer) != null)
        {
            animator.IsMoving = false;
            OnStartedBattle();
        }
    }


}
