using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] HPBar hpBar;

    EnemyBattle enemy;

    public void SetData(EnemyBattle enemy)
    {
        this.enemy = enemy;
        nameText.text = enemy.Name;
        hpBar.SetHP((float)enemy.HP / enemy.MaxHP);
    }
    public void UpdateHP()
    {
        hpBar.SetHP((float)enemy.HP / enemy.MaxHP);
    }
}
