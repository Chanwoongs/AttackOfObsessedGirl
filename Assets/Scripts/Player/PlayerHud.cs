using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] HPBar hpBar;

    private YeonwooBattle yeonwoo;

    public void SetData(YeonwooBattle yeonwoo)
    {
        this.yeonwoo = yeonwoo;
        nameText.text = yeonwoo.Name;
        hpBar.SetHP((float)yeonwoo.HP / yeonwoo.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)yeonwoo.HP / yeonwoo.MaxHP);
    }
}
