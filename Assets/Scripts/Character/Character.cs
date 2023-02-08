using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterAnimator animator;


    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public CharacterAnimator Animator
    {
        get => animator;
    }

    public void HandleUpdate()
    {

    }

    public void LookTowards(Vector3 targetPos)
    {
        var xDiff = targetPos.x - transform.position.x;
        if (xDiff > 0)
        {
            animator.FacingDir = Vector3.left;
        }
        else if (xDiff < 0)
        {
            animator.FacingDir = Vector3.right;
        }




    }
}
