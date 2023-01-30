using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    string Name { get; set; }

    string TriggerText { get; set; }
    string HitText { get; set; }

    int Effect { get; set; }
}
