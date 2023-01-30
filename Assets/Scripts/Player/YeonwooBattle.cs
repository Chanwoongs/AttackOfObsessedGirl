using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YeonwooBattle : MonoBehaviour, IBattleCharacterBase
{
    public string Name { get; set; }
    public string Text { get; set; }
    public Image Img { get; set; }
    public int MaxHP { get; set; }
    public int HP { get; set; }
    public int Damage { get; set; }

    public List<GameObject> skills;

    public void Awake()
    {
        Name = "Yeonwoo";
        Text = "This is Test";
        MaxHP = 100;
        Damage = 10;
        HP = MaxHP;
    }

    public void SetUp()
    {
        Img = transform.GetChild(0).GetComponent<Image>();
        Img.sprite = Resources.Load<Sprite>("TemporaryAssets/Art/Trianers/Brendan_Back");
    }

}