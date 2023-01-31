using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleActionType
{
    Attack, DamageBuff, HealthBuff
}

public class BattleActionComponent : MonoBehaviour
{
    [SerializeField] private string actionName;
    [SerializeField] private string triggerText;
    [SerializeField] private string hitText;
    [SerializeField] private int effectAmount;
    [SerializeField] private BattleActionType type;

    public string GetActionName() { return actionName; }
    public string GetTriggerText() { return triggerText; }
    public string GetHitText() { return hitText; }
    public int GetEffectAmount() { return effectAmount; }
    public BattleActionType GetActionType() { return type; }

    public void IncreaseEffectAmount(int amount) { effectAmount += amount; }
}
