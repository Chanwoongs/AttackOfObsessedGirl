using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> idleLeftSprites;
    [SerializeField] List<Sprite> idleRightSprites;

    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;

    [SerializeField] List<Sprite> danceSprites;

    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool IsDancing { get; set; }
    public Vector3 FacingDir { get; set; }

    // States
    SpriteAnimator idleLeftAnim;
    SpriteAnimator idleRightAnim;

    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;

    SpriteAnimator danceAnim;
    SpriteAnimator currentAnim;

    // References
    SpriteRenderer spriteRenderer;

    bool wasPreviouslyMoving;

    private void Start()
    {
        FacingDir = Vector3.left;

        spriteRenderer = GetComponent<SpriteRenderer>();
        idleLeftAnim = new SpriteAnimator(idleLeftSprites, spriteRenderer, 0.1f);
        idleRightAnim = new SpriteAnimator(idleRightSprites, spriteRenderer, 0.1f);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer, 0.1f);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer, 0.1f);
        danceAnim = new SpriteAnimator(danceSprites, spriteRenderer, 0.1f);

        currentAnim = walkLeftAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;

        if (FacingDir == Vector3.right && MoveX == 0f && MoveY == 0f)
            currentAnim = idleRightAnim;
        else if (FacingDir == Vector3.left && MoveX == 0f && MoveY == 0f)
            currentAnim = idleLeftAnim;
        else if ((MoveX == 1f || (Mathf.Abs(MoveY) > 0 && FacingDir == Vector3.right)) && IsMoving)
        {
            currentAnim = walkRightAnim;
            FacingDir = Vector3.right;
        }
        else if ((MoveX == -1f || (Mathf.Abs(MoveY) > 0 && FacingDir == Vector3.left)) && IsMoving)
        { 
            currentAnim = walkLeftAnim;
            FacingDir = Vector3.left;
        }
        
        if (IsDancing)
            currentAnim = danceAnim;

        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        if(currentAnim.Frames.Count > 0)
            currentAnim.HandleUpdate();

        wasPreviouslyMoving = IsMoving;
    }
}
