using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MGDoor : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] GameObject MGInside;

    private bool isVisited;

    public void OnPlayerTriggered(PlayerController player)
    {
        if (isVisited) return;

        isVisited = true;
        player.StopPlayer();
        StartCoroutine(MovePlayer(player));
    }

    private IEnumerator MovePlayer(PlayerController player)
    {
        GameController.Instance.StopUpdate();
        
        // UI 끄기
        GameController.Instance.HpBar.gameObject.SetActive(false);
        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartDescendingTransition();

        player.transform.position = MGInside.transform.position;

        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartAscendingTransition();
        GameController.Instance.HpBar.gameObject.SetActive(true);

        // 회사 사람들 말하는 코루틴
        yield return new WaitForSeconds(3f);
        GameController.Instance.MGChangeUI();
        player.SetSpeed(8);
        yield return new WaitForSeconds(3f);

        GameController.Instance.HpBar.gameObject.SetActive(false);
        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartDescendingTransition();

        // 다시 플레이어 옮기기
        player.transform.position = new Vector3(transform.position.x, transform.position.y - 1);

        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartAscendingTransition();
        // UI 켜기
        GameController.Instance.HpBar.gameObject.SetActive(true);

        GameController.Instance.ResumeFreeRoamUpdate();
    }
}
