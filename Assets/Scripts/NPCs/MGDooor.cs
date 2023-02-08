using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGDooor : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        Debug.Log("Enter MG Company");
    }
}
