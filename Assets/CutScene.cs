using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class CutScene : MonoBehaviour
{
    public GameObject firstCamera;
    public CinemachineSplineDolly dollyCamera;

    public GameObject activateGhost;
    public GameObject deactivateGhost;
    public GameObject[] activateUI;
    public PlayerInput playerInput;

    private void Start()
    {
        // player input ����
        playerInput.enabled = false;

        // 5�� �� firstCamera ���� dollyCamera�� camera position �̵�
        StartCoroutine(ActivateDolly());

        // UI ��Ȱ��ȭ
        for (int i = 0; i < activateUI.Length; i++)
            activateUI[i].SetActive(false);

        // activateGhost ��Ȱ��ȭ
        activateGhost.SetActive(false);
    }

    IEnumerator ActivateDolly()
    {
        yield return new WaitForSeconds(5f);
        firstCamera.SetActive(false);

        // firstCamera -> dollyCamera�� �̵� ��ٸ���
        yield return new WaitForSeconds(1f);

        // dotween���� dollyCamera.CameraPosition 1�� ���� ����
        yield return DOTween.To(() => dollyCamera.CameraPosition, x => dollyCamera.CameraPosition = x, 1, 10f).SetEase(Ease.Linear).WaitForCompletion();

        // dollyCamera GameObject ��Ȱ��ȭ ����, activateGhost Ȱ��ȭ + deactivateGhost ��Ȱ��ȭ
        dollyCamera.gameObject.SetActive(false);
        activateGhost.SetActive(true);
        deactivateGhost.SetActive(false);

        for(int i = 0; i < activateUI.Length; i++)
            activateUI[i].SetActive(true);

        // player input Ǯ��
        playerInput.enabled = true;
    }
}
