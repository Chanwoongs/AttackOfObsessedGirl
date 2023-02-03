using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;

    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    // States
    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;

    SpriteAnimator currentAnim;

    // References
    SpriteRenderer spriteRenderer;

    bool wasPreviouslyMoving;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer, 0.1f);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer, 0.1f);

        currentAnim = walkLeftAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;

        if (MoveX == 1f)
            currentAnim = walkRightAnim;
        else if (MoveX == -1f)
            currentAnim = walkLeftAnim;

        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.Frames[0];

        wasPreviouslyMoving = IsMoving;
    }
}
