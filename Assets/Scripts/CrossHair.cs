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
    public float m_duration = 0.2f;       // �� �� ������ ��

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

    // Gun.cs�� Fire() �Լ����� ���� ������ �� ȣ��
    // hitCrossHair�� Ȱ��ȭ�ϰ� 0.1�� �� ��Ȱ��ȭ
    public void Hit()
    {
        m_isHit = true;
        m_elapsedTime = 0.0f;

        m_fadeTweener.ChangeEndValue(new Color(1.0f, 0.0f, 0.0f, 1.0f), true).Restart();
        m_scaleTweener.ChangeEndValue(Vector3.one * 1.2f, true).Restart();
    }

    // m_elapsedTime��ŭ ��ٸ���, ����ؼ� alpha���� scale�� ���ҽ�Ŵ
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
