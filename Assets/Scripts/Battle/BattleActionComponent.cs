using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleActionType
{
    Attack, SpecialAttack, DamageBuff, HealthBuff,
}

public class BattleActionComponent : MonoBehaviour
{
    [SerializeField] private string actionName;
    [SerializeField] private string triggerText;
    [SerializeField] private string hitText;
    [SerializeField] private int effectAmount;
    [SerializeField] private BattleAction action;
    [SerializeField] private BattleActionType type;
    [SerializeField] private int bad;
    [SerializeField] private int good;
    [SerializeField] private int great;
    [SerializeField] private float goodRatio;
    [SerializeField] private float greatRatio;

    public string ActionName { get => actionName; }
    public string TriggerText { get => triggerText; }
    public string HitText { get => hitText; }
    public int EffectAmount { get => effectAmount; }
    public BattleAction Action { get => action; }
    public BattleActionType Type { get => type; }
    public int Bad { get => bad; }
    public int Good { get => good; }
    public int Great { get => great; }
    public float GoodRatio { get => goodRatio; }  
    public float GreatRatio { get => greatRatio; }
    public void IncreaseEffectAmount(int amount) { effectAmount += amount; }
}
