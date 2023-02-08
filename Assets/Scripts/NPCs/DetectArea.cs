using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectArea : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        if (player.IsDanceOver) return;

        if (Mathf.Abs(this.GetComponentInParent<Transform>().position.x - player.transform.position.x) < 0.05f &&
          !this.GetComponentInParent<DetectNPCController>().HasDetected)
        {
            GameController.Instance.OnDetected(GetComponentInParent<DetectNPCController>());
        }
    }
}
