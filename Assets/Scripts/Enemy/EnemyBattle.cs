using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattle : MonoBehaviour, IBattleCharacterBase
{
    public string Name { get; set; }
    public string Text { get; set; }
    public Image Img { get; set; }
    public int MaxHP { get; set; }
    public int HP { get; set; }
    public int Damage { get; set; }

    [SerializeField] private GameObject action;

    public void Awake()
    {
        Name = "ObsessedGirl";
        Text = "This is Test";
        MaxHP = 100;
        Damage = 10;
        HP = MaxHP;
    }

    public void SetUp()
    {
        Img = transform.GetChild(0).GetComponent<Image>();
        Img.sprite = Resources.Load<Sprite>("TemporaryAssets/Art/Trianers/CoolTrainer_M");
    }

    public GameObject GetAction() { return action; }

    public bool TakeAction(BattleActionComponent action)
    {
        switch (action.GetActionType())
        {
            case BattleActionType.Attack:
                HP -= action.GetEffectAmount();
                if (MaxHP - HP > 30 && HP < 10)
                {
                    MaxHP = HP;
                    return true;
                }
                break;
            case BattleActionType.DamageBuff:
                this.action.GetComponent<BattleActionComponent>().IncreaseEffectAmount(action.GetEffectAmount());
                break;
            case BattleActionType.HealthBuff:
                HP += action.GetEffectAmount();
                break;
            default:
                break;
        }
        return false;
    }
}