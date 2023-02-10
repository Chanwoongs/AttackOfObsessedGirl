using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MGDoor : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] GameObject MGInside;

    public event Action OnPlayerVisit;
    public event Action OnPlayerExit;
    public event Action OnChangeUI;

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
        OnPlayerVisit();
        
        // UI ����
        GameController.Instance.HpBar.gameObject.SetActive(false);
        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartDescendingTransition();

        player.transform.position = MGInside.transform.position;

        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartAscendingTransition();
        GameController.Instance.HpBar.gameObject.SetActive(true);

        // ȸ�� ����� ���ϴ� �ڷ�ƾ
        yield return new WaitForSeconds(3f);
        OnChangeUI();
        player.SetSpeed(8);
        yield return new WaitForSeconds(3f);

        GameController.Instance.HpBar.gameObject.SetActive(false);
        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartDescendingTransition();

        // �ٽ� �÷��̾� �ű��
        player.transform.position = new Vector3(transform.position.x, transform.position.y - 1);

        yield return GameController.Instance.Transitions.GetComponent<CircularTransition>().StartAscendingTransition();
        // UI �ѱ�
        GameController.Instance.HpBar.gameObject.SetActive(true);

        OnPlayerExit();
    }
}
