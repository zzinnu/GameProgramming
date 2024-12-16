using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CrossHair : MonoBehaviour
{
    public GameObject hitCrossHair;
    public float m_duration = 0.2f;       // 몇 초 유지할 지

    private Image m_hitCrossHairImage;
    private Tweener m_fadeTweener;
    private Tweener m_scaleTweener;
    private bool m_isHit = false;
    private float m_elapsedTime = 0.0f;

    private void Awake()
    {
        if (hitCrossHair == null)
            hitCrossHair = transform.Find("Hit").gameObject;

        m_hitCrossHairImage = hitCrossHair.GetComponent<Image>();

        m_fadeTweener = m_hitCrossHairImage.DOFade(1.0f, 0.5f)
                                            .SetEase(Ease.OutCubic)
                                            .Pause()
                                            .SetAutoKill(false);

        m_scaleTweener = hitCrossHair.transform.DOScale(1.2f, 0.2f)
                                                .SetEase(Ease.InOutSine)
                                                .Pause()
                                                .SetAutoKill(false);
    }

    // Gun.cs의 Fire() 함수에서 적을 맞췄을 때 호출
    // hitCrossHair를 활성화하고 0.1초 후 비활성화
    public void Hit()
    {
        m_isHit = true;
        m_elapsedTime = 0.0f;

        m_fadeTweener.ChangeEndValue(new Color(1.0f, 0.0f, 0.0f, 1.0f), true).Restart();
        m_scaleTweener.ChangeEndValue(Vector3.one * 1.2f, true).Restart();
    }

    // m_elapsedTime만큼 기다리고, 계속해서 alpha값과 scale을 감소시킴
    private void Update()
    {
        if(m_isHit)
        {
            m_elapsedTime += Time.deltaTime;

            if(m_elapsedTime >= m_duration)
            {
                m_isHit = false;
                m_elapsedTime = 0.0f;

                m_fadeTweener.ChangeEndValue(new Color(1.0f, 0.0f, 0.0f, 0.0f), true).Restart();
                m_scaleTweener.ChangeEndValue(Vector3.one, true).Restart();
            }
        }
    }

    private void OnDestroy()
    {
        m_fadeTweener.Kill();
        m_scaleTweener.Kill();
    }
}
