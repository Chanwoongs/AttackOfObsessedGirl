using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1.0f);   
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float curHP = health.transform.localScale.x;
        float changeAmount = curHP - newHP;

        if (changeAmount >= 0f)
        {
            while (curHP - newHP > Mathf.Epsilon)
            {
                curHP -= Mathf.Abs(changeAmount) * Time.deltaTime;
                health.transform.localScale = new Vector3(curHP, 1f);
                yield return null;
            }
        }
        else if (changeAmount < 0f)
        {
            while (curHP - newHP < Mathf.Epsilon)
            {
                curHP += Mathf.Abs(changeAmount) * Time.deltaTime;
                health.transform.localScale = new Vector3(curHP, 1f);
                yield return null;
            }
        }
        health.transform.localScale = new Vector3(newHP, 1f);
    }

    public void ChangeHealthBar()
    {
        health.GetComponent<Image>().color = Color.cyan;
    }
}
