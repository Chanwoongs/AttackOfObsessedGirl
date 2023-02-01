using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum Action
{
    Attack, Book, Bottle, Burger, LipStick
}


public class YeonwooBattle : MonoBehaviour, IBattleCharacterBase
{
    public string Name { get; set; }
    public string Text { get; set; }
    public Image Img { get; set; }
    public int MaxHP { get; set; }
    public int HP { get; set; }
    public int Damage { get; set; }
    public Vector3 OriginalImagePos { get; set; }
    public Color OriginalImageColor { get; set; }

    [SerializeField] private List<GameObject> actions;
    [SerializeField] private List<GameObject> currentActions;

    public void Awake()
    {
        Name = "Yeonwoo";
        Text = "This is Test";
        MaxHP = 100;
        Damage = 10;
        HP = MaxHP; 
        
        Img = transform.GetChild(0).GetComponent<Image>();
        Img.sprite = Resources.Load<Sprite>("TemporaryAssets/Art/Trianers/Brendan_Back");

        OriginalImagePos = Img.transform.localPosition;
        OriginalImageColor = Img.color;
    }

    public void SetUp()
    {
        PlayEnterAnimation();
        SetUpActions();
    }

    public void SetUpActions()
    {
        // 얻은 아이템들을 체크하여 스킬에 넣어주기
        /*
         * if (hasBurger) currentSkills.add(skills[(int)Skill.Burger]);
         */
        // 임시로 넣어 놓기
        currentActions.Add(actions[(int)Action.Attack]);
        currentActions.Add(actions[(int)Action.Book]);
        currentActions.Add(actions[(int)Action.LipStick]);
    }

    public List<GameObject> GetActions() { return actions; }
    public List<GameObject> GetCurrentActions() { return currentActions; }

    public bool TakeAction(BattleActionComponent action)
    {
        HP -= action.GetEffectAmount();
        if (HP <= 0) return true;
        return false;
    }

    public void PlayEnterAnimation()
    {
        Img.transform.localPosition = new Vector3(-500f, OriginalImagePos.y);
        Img.transform.DOLocalMoveX(OriginalImagePos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(Img.transform.DOLocalMoveX(OriginalImagePos.x + 50f, 0.25f));
        sequence.Append(Img.transform.DOLocalMoveX(OriginalImagePos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(Img.DOColor(Color.gray, 0.1f));
        sequence.Append(Img.DOColor(OriginalImageColor, 0.1f));

        Img.transform.DOShakePosition(1f, 3f);
    }

    public void PlayLoseAnimation()
    {
        // 쓰러지는 애니메이션
    }
}