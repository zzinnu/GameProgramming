using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class LifeUI : MonoBehaviour
{
    public int lifeperImage = 10;
    public int width = 300;                         // GridLayoutGroup의 width값 : lifeperImage * gridLayoutGroup.cellSize.x로 계산
    public GameObject lifePrefab;
    public UnityEvent onChanged;

    [SerializeField] private List<GameObject> m_lifePrefabList = new List<GameObject>();
    private GridLayoutGroup m_gridLayoutGroup;
    private int m_currentPrefabCount;
    private float m_currentRemain;


    private void Awake()
    {
        m_gridLayoutGroup = GetComponent<GridLayoutGroup>();

        onChanged.RemoveAllListeners();
        onChanged.AddListener(OnUpdate);
    }

    private void OnDestroy()
    {
        onChanged.RemoveAllListeners();
    }

    private void Start()
    {
        // 기존 lifePrefab을 모두 삭제
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int prefabCount = 0;                                    // life prefab의 개수
        float remain = 0.0f;                                    // 마지막 life prefab의 fill amount
        CalculatePrefabCount(ref prefabCount, ref remain);

        InstantiateLifePrefab(prefabCount, remain);
    }

    private void Update()
    {
        OnUpdate();
    }

    private void CalculatePrefabCount(ref int prefabCount, ref float remain)
    {
        // Player의 life와 lifeperImage값을 기반으로 lifePrefab을 생성하여 m_lifeList에 추가
        float playerLife = GameManager.Instance.Player.playerLife.CurrentLifes;
        prefabCount = Mathf.Clamp(Mathf.CeilToInt(playerLife / lifeperImage), 0, 10000);    // 10000은 적당한 큰 수
        m_currentPrefabCount = prefabCount;

        // fill amount를 계산하기 위해 남은 life를 저장
        remain = Mathf.Clamp(playerLife - prefabCount * lifeperImage, 0, lifeperImage);
        m_currentRemain = remain;

        // Debug.Log("current life : " + playerLife + " / prefabCount : " + prefabCount + " / remain : " + remain);
    }

    private void InstantiateLifePrefab(int prefabCount, float remain)
    {
        // prefabCount만큼 Life prefab 생성
        for (int i = m_lifePrefabList.Count; i < prefabCount; i++)
        {
            GameObject life = Instantiate(lifePrefab, transform);
            m_lifePrefabList.Add(life);
        }

        // remain이 있다면 fill amount를 계산하여 마지막 Life prefab 생성
        if (remain > 0)
        {
            GameObject life = Instantiate(lifePrefab, transform);
            Image image = life.GetComponent<Image>();
            image.fillAmount = remain;
            m_lifePrefabList.Add(life);
        }

        // GridLayoutGroup의 width값을 계산, Size 조절
        int denom = (prefabCount + (remain > 0 ? 1 : 0));
        m_gridLayoutGroup.cellSize = new Vector2(width / denom, m_gridLayoutGroup.cellSize.y);
    }

    private void OnUpdate()
    {
        // current life 정보 저장
        int prefabCount_prev = m_currentPrefabCount;
        float remain_prev = m_currentRemain;

        // current life 정보 갱신
        int prefabCount = 0;
        float remain = 0.0f;
        CalculatePrefabCount(ref prefabCount, ref remain);
        // Debug.Log("prev : " + prefabCount_prev + " / current : " + prefabCount);

        // prev와 current를 비교하여 lifePrefab을 추가하거나 제거
        // prev < current : lifePrefab 추가
        if (prefabCount_prev < prefabCount)
        {
            for (int i = prefabCount_prev; i < prefabCount; i++)
            {
                GameObject life = Instantiate(lifePrefab, transform);
                m_lifePrefabList.Add(life);
            }
        }

        // prev > current : lifePrefab 제거
        else if (prefabCount_prev > prefabCount)
        {
            for (int i = prefabCount_prev - 1; i >= prefabCount; i--)
            {
                DestroyLifePrefab(m_lifePrefabList[i]);
            }
        }

        // prev == current : 마지막 lifePrefab의 fill amount만 변경
        if (remain_prev != remain)
        {
            Image image = m_lifePrefabList[m_lifePrefabList.Count - 1].GetComponent<Image>();
            image.fillAmount = remain;
        }

    }

    private void DestroyLifePrefab(GameObject lifePrefab)
    {
        // lifePrefab의 Image의 Color를 red로 바꾸고, FadeOut, ScaleUp한 후 Destroy
        Image image = lifePrefab.GetComponent<Image>();

        // DoTween을 사용하여 lifePrefab의 Image의 Color를 red로 바꾸고, FadeOut
        image.DOColor(Color.red, 0.5f);
        image.DOFade(0, 0.5f);

        // y축으로 ScaleUp
        image.transform.DOScaleY(3.0f, 0.5f).OnComplete(() => 
        {
            m_lifePrefabList.Remove(lifePrefab);
            Destroy(lifePrefab);
        });
    }
}
