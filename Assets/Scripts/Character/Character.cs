using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteState
{
    Idle, Joy, Angry, Sad, Tired, Max,
}

public class Character : MonoBehaviour
{
    CharacterAnimator animator;
    [SerializeField] List<Sprite> sprites;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public void HandleUpdate()
    {

    }

    public void LookTowards(Vector3 targetPos)
    {
        var xDiff = targetPos.x - transform.position.x;

        if (xDiff > 0)
            animator.FacingDir = Vector3.left;
        else if (xDiff < 0)
            animator.FacingDir = Vector3.right;
    }

    public CharacterAnimator Animator { get => animator; }

    public List<Sprite> Sprites { get => sprites; }
}
