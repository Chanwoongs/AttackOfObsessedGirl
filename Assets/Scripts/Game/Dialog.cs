using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SwitchingData
{
    public int switchingLineNum;

    public SpriteState leftState;
    public SpriteState rightState;

    public SwitchingData(int switchingLineNum = -1, SpriteState leftState = SpriteState.Idle, SpriteState rightState = SpriteState.Idle)
    {
        this.switchingLineNum = switchingLineNum;
        this.leftState = leftState;
        this.rightState = rightState;
    }
}

[System.Serializable]
public class Dialog 
{
    [SerializeField] List<string> lines;

    public List<SwitchingData> switchingDatas = new List<SwitchingData>();

    public List<string> Lines { get => lines; }
    public List<SwitchingData> SwitchingDatas { get => switchingDatas; } 
}
