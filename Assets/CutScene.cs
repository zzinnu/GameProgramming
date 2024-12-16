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
        // player input 막기
        playerInput.enabled = false;

        // 5초 뒤 firstCamera 끄고 dollyCamera의 camera position 이동
        StartCoroutine(ActivateDolly());

        // UI 비활성화
        for (int i = 0; i < activateUI.Length; i++)
            activateUI[i].SetActive(false);

        // activateGhost 비활성화
        activateGhost.SetActive(false);
    }

    IEnumerator ActivateDolly()
    {
        yield return new WaitForSeconds(5f);
        firstCamera.SetActive(false);

        // firstCamera -> dollyCamera로 이동 기다리기
        yield return new WaitForSeconds(1f);

        // dotween으로 dollyCamera.CameraPosition 1로 점점 증가
        yield return DOTween.To(() => dollyCamera.CameraPosition, x => dollyCamera.CameraPosition = x, 1, 10f).SetEase(Ease.Linear).WaitForCompletion();

        // dollyCamera GameObject 비활성화 이후, activateGhost 활성화 + deactivateGhost 비활성화
        dollyCamera.gameObject.SetActive(false);
        activateGhost.SetActive(true);
        deactivateGhost.SetActive(false);

        for(int i = 0; i < activateUI.Length; i++)
            activateUI[i].SetActive(true);

        // player input 풀기
        playerInput.enabled = true;
    }
}
