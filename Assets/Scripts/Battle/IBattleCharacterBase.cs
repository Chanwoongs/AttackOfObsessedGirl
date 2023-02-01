using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBattleCharacterBase
{
    string Name { get; set; }
    string Text { get; set; }
    Image Img { get; set; }
    int MaxHP { get; set; }
    int HP { get; set; }
    int Damage { get; set; }

    Vector3 OriginalImagePos { get; set; }
    Color OriginalImageColor { get; set; }

    bool TakeAction(BattleActionComponent action);

    void PlayEnterAnimation();
    void PlayAttackAnimation();
    void PlayHitAnimation();  
    void PlayLoseAnimation();  
}
